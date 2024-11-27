using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using static VirtoCommerce.CommunicationModule.Core.ModuleConstants;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class MessageService : IMessageService
{
    private readonly Func<ICommunicationRepository> _repositoryFactory;
    private readonly ISettingsManager _settingsManager;
    private readonly IMessageSenderRegistrar _messageSenderRegistrar;
    private readonly IMessageCrudService _messageCrudService;
    private readonly IConversationCrudService _conversationCrudService;

    public MessageService(
        Func<ICommunicationRepository> repositoryFactory,
        ISettingsManager settingsManager,
        IMessageSenderRegistrar messageSenderRegistrar,
        IMessageCrudService messageCrudService,
        IConversationCrudService conversationCrudService
        )
    {
        _repositoryFactory = repositoryFactory;
        _settingsManager = settingsManager;
        _messageSenderRegistrar = messageSenderRegistrar;
        _messageCrudService = messageCrudService;
        _conversationCrudService = conversationCrudService;
    }

    public virtual async Task<IList<Message>> GetMessagesByConversation(string conversationId)
    {
        var result = new List<Message>();
        using var repository = _repositoryFactory();

        var messageEntities = await repository.GetMessagesByConversationAsync(conversationId);

        if (messageEntities != null && messageEntities.Any())
        {
            foreach (var messageEntity in messageEntities)
            {
                var message = messageEntity.ToModel(AbstractTypeFactory<Message>.TryCreateInstance());
                result.Add(message);
            }
        }

        return result;
    }

    public virtual async Task<IList<Message>> GetMessagesByThread(string threadId)
    {
        var result = new List<Message>();
        using var repository = _repositoryFactory();

        var messageEntities = await repository.GetMessagesByThreadAsync(threadId, MessageResponseGroup.None.ToString());

        if (messageEntities != null && messageEntities.Any())
        {
            foreach (var messageEntity in messageEntities)
            {
                var message = messageEntity.ToModel(AbstractTypeFactory<Message>.TryCreateInstance());
                result.Add(message);
            }
        }

        return result;
    }

    public virtual async Task<IList<Message>> GetThread(string threadId)
    {
        var result = new List<Message>();
        var threadIdToSearch = threadId;

        while (!string.IsNullOrEmpty(threadIdToSearch))
        {
            var message = await _messageCrudService.GetByIdAsync(threadIdToSearch, MessageResponseGroup.WithoutAnswers.ToString());
            threadIdToSearch = message?.ThreadId;

            if (message != null)
            {
                result.Add(message);
            }
        }

        return result;
    }

    public virtual async Task SendMessage(Message message)
    {
        if (string.IsNullOrEmpty(message.Id))
        {
            message.Id = Guid.NewGuid().ToString();
        }

        await _messageCrudService.SaveChangesAsync([message]);

        var conversation = await _conversationCrudService.GetByIdAsync(message.ConversationId);
        if (conversation != null)
        {
            conversation.LastMessageId = message.Id;
            conversation.LastMessageTimestamp = DateTime.UtcNow;
            await _conversationCrudService.SaveChangesAsync([conversation]);
        }

        var messageSenders = await _settingsManager.GetValueAsync<string>(Settings.General.MessageSenders);

        if (messageSenders != null)
        {
            var sender = _messageSenderRegistrar.AllRegisteredSenders.FirstOrDefault(x => x.SenderName == messageSenders);
            if (sender != null)
            {
                await sender.SendMessage(message);
            }
        }
    }

    public virtual async Task UpdateMessage(Message message)
    {
        // TODO: more logic with sender?
        await _messageCrudService.SaveChangesAsync([message]);
    }

    public virtual async Task DeleteMessage(IList<string> messageIds, bool withReplies)
    {
        var messages = await _messageCrudService.GetAsync(messageIds);
        var conversationIds = messages.Select(x => x.ConversationId).Distinct().ToList();

        if (withReplies)
        {
            var idsToDelete = await GetChildMessageIdsRecursively(messageIds);
            await _messageCrudService.DeleteAsync(idsToDelete);
        }
        else
        {
            foreach (var messageId in messageIds)
            {
                var childMessages = await GetMessagesByThread(messageId);
                foreach (var childMessage in childMessages)
                {
                    childMessage.ThreadId = null;
                }

                await _messageCrudService.SaveChangesAsync(childMessages);
            }

            await _messageCrudService.DeleteAsync(messageIds);
        }

        var conversations = await _conversationCrudService.GetAsync(conversationIds);
        foreach (var conversation in conversations)
        {
            if (conversation != null && messageIds.Contains(conversation.LastMessageId))
            {
                var newLastMessage = (await GetMessagesByConversation(conversation.Id)).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                conversation.LastMessageId = newLastMessage?.Id;
                conversation.LastMessageTimestamp = newLastMessage?.CreatedDate ?? DateTime.MinValue;
            }
        }

        await _conversationCrudService.SaveChangesAsync(conversations);
    }

    public virtual async Task<Message> SetMessageReadStatus(string messageId, string recipientId, bool notRead = false)
    {
        var message = await _messageCrudService.GetByIdAsync(messageId);
        if (message == null)
        {
            throw new InvalidOperationException($"Message with id {messageId} not found");
        }

        var recipient = message.Recipients?.FirstOrDefault(x => x.RecipientId == recipientId);
        if (recipient == null)
        {
            throw new InvalidOperationException($"Recipient with id {recipientId} not received message {messageId}");
        }

        if (notRead)
        {
            recipient.ReadStatus = ReadStatus.NotRead;
        }
        else
        {
            recipient.ReadStatus = ReadStatus.Read;
            recipient.ReadTimestamp = DateTime.UtcNow;
        }

        await _messageCrudService.SaveChangesAsync([message]);

        return message;
    }

    public virtual async Task<Message> SetMessageReaction(string messageId, string userId, string reaction)
    {
        var message = await _messageCrudService.GetByIdAsync(messageId);
        if (message == null)
        {
            throw new InvalidOperationException($"Message with id {messageId} not found");
        }

        var messageReaction = message.Reactions?.FirstOrDefault(x => x.UserId == userId);
        if (messageReaction == null)
        {
            messageReaction = AbstractTypeFactory<MessageReaction>.TryCreateInstance();
            messageReaction.MessageId = messageId;
            messageReaction.UserId = userId;
            messageReaction.Reaction = reaction;
            if (message.Reactions == null)
            {
                message.Reactions = new List<MessageReaction>();
            }
            message.Reactions.Add(messageReaction);
        }
        else
        {
            messageReaction.Reaction = reaction;
        }

        await _messageCrudService.SaveChangesAsync([message]);

        return message;
    }

    public virtual async Task<int> GetUnreadMessagesCount(string recipientId, string conversationId)
    {
        int result = 0;

        if (!string.IsNullOrEmpty(recipientId) && !string.IsNullOrEmpty(conversationId))
        {

            var messages = await GetMessagesByConversation(conversationId);

            if (messages != null && messages.Any())
            {
                var unreadMessagesCount = messages.Where(x => x.Recipients.Any(r => r.ReadStatus != ReadStatus.Read && r.RecipientId == recipientId)).Count();
                result = unreadMessagesCount;
            }
        }

        return result;
    }

    protected virtual async Task<List<string>> GetChildMessageIdsRecursively(IList<string> parendMessageIds)
    {
        foreach (var parendMessageId in parendMessageIds)
        {
            var childMessages = (await GetMessagesByThread(parendMessageId)).Select(x => x.Id).ToList();
            if (childMessages.Any())
            {
                childMessages.AddRange(await GetChildMessageIdsRecursively(childMessages));
                return childMessages;
            }
            else
            {
                return new List<string>([parendMessageId]);
            }
        }

        return new List<string>();
    }
}
