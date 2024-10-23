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

    public MessageService(
        Func<ICommunicationRepository> repositoryFactory,
        ISettingsManager settingsManager,
        IMessageSenderRegistrar messageSenderRegistrar,
        IMessageCrudService messageCrudService
        )
    {
        _repositoryFactory = repositoryFactory;
        _settingsManager = settingsManager;
        _messageSenderRegistrar = messageSenderRegistrar;
        _messageCrudService = messageCrudService;
    }

    public virtual async Task<IList<Message>> GetMessagesByEntity(string entityId, string entityType)
    {
        var result = new List<Message>();
        using var repository = _repositoryFactory();

        var messageEntities = await repository.GetMessagesByEntityAsync(entityId, entityType);

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

    public virtual Task<IList<Message>> GetMessagesByThread(string threadId)
    {
        throw new NotImplementedException();
    }

    //public virtual async Task<IList<Message>> GetMessagesBySender(string senderId)
    //{
    //    return await GetMessagesByCondition(x => x.SenderId == senderId);
    //}

    public virtual async Task SendMessage(Message message)
    {
        await _messageCrudService.SaveChangesAsync([message]);

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

    public virtual async Task<Message> SetMessageReadStatus(string messageId, string recipientId, bool notRead = false)
    {
        var message = await _messageCrudService.GetByIdAsync(messageId);
        if (message == null)
        {
            throw new InvalidOperationException($"Message with id {messageId} not found");
        }

        var recipient = message.Recipients.FirstOrDefault(x => x.Id == recipientId);
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

        var messageReaction = message.Reactions.FirstOrDefault(x => x.UserId == userId);
        if (messageReaction == null)
        {
            messageReaction = AbstractTypeFactory<MessageReaction>.TryCreateInstance();
            messageReaction.Reaction = reaction;
            message.Reactions.Add(messageReaction);
        }
        else
        {
            messageReaction.Reaction = reaction;
        }

        await _messageCrudService.SaveChangesAsync([message]);

        return message;
    }
}
