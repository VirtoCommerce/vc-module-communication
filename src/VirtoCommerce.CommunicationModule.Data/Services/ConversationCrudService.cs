using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class ConversationCrudService : CrudService<Conversation, ConversationEntity,
    GenericChangedEntryEvent<Conversation>, GenericChangedEntryEvent<Conversation>>,
    IConversationCrudService
{
    private readonly Func<ICommunicationRepository> _repositoryFactory;
    private readonly IMessageCrudService _messageCrudService;
    private readonly IMessageService _messageService;

    public ConversationCrudService(
        Func<ICommunicationRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IEventPublisher eventPublisher,
        IMessageCrudService messageCrudService,
        IMessageService messageService
        ) : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        _repositoryFactory = repositoryFactory;
        _messageCrudService = messageCrudService;
        _messageService = messageService;
    }

    protected override Task<IList<ConversationEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((ICommunicationRepository)repository).GetConversationsByIdsAsync(ids, responseGroup);
    }

    public override async Task<IList<Conversation>> GetAsync(IList<string> ids, string responseGroup = null, bool clone = true)
    {
        var conversations = (await base.GetAsync(ids.ToList(), responseGroup)).ToArray();

        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, ConversationResponseGroup.None);

        if (respGroupEnum.HasFlag(ConversationResponseGroup.WithLastMessage))
        {
            if (!conversations.IsNullOrEmpty())
            {
                var lastMessageIds = conversations.Select(x => x.LastMessageId).ToList();
                var lastMessages = await _messageCrudService.GetAsync(lastMessageIds);

                foreach (var conversation in conversations)
                {
                    conversation.LastMessage = lastMessages.FirstOrDefault(x => x.Id == conversation.LastMessageId);
                }
            }
        }

        if (respGroupEnum.HasFlag(ConversationResponseGroup.WithUnreadCount))
        {
            if (!conversations.IsNullOrEmpty())
            {
                foreach (var conversation in conversations)
                {
                    conversation.UnreadMessagesCount = await _messageService.GetUnreadMessagesCount(conversation.Users.FirstOrDefault()?.UserId, conversation.Id);
                }
            }
        }

        return conversations;
    }
}
