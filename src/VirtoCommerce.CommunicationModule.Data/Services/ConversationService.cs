using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class ConversationService : IConversationService
{
    private readonly Func<ICommunicationRepository> _repositoryFactory;
    private readonly ICommunicationUserCrudService _communicationUserCrudService;
    private readonly IConversationCrudService _conversationCrudService;

    public ConversationService(
        Func<ICommunicationRepository> repositoryFactory,
        ICommunicationUserCrudService communicationUserCrudService,
        IConversationCrudService conversationCrudService
        )
    {
        _repositoryFactory = repositoryFactory;
        _communicationUserCrudService = communicationUserCrudService;
        _conversationCrudService = conversationCrudService;
    }

    public virtual async Task<Conversation> GetConversationByEntity(string entityId, string entityType)
    {
        Conversation conversation = null;
        using var repository = _repositoryFactory();

        var conversationEntitiy = await repository.GetConversationByEntityAsync(entityId, entityType);

        if (conversationEntitiy != null)
        {
            conversation = conversationEntitiy.ToModel(AbstractTypeFactory<Conversation>.TryCreateInstance());
        }

        return conversation;
    }

    public virtual async Task<Conversation> GetConversationByUsers(IList<string> userIds)
    {
        Conversation conversation = null;
        using var repository = _repositoryFactory();

        var conversationEntitiy = await repository.GetConversationByUsersAsync(userIds);

        if (conversationEntitiy != null)
        {
            conversation = conversationEntitiy.ToModel(AbstractTypeFactory<Conversation>.TryCreateInstance());
        }

        return conversation;
    }

    public virtual async Task<Conversation> CreateConversation(IList<string> userIds, string name, string iconUrl, string entityId, string entityType)
    {
        if (userIds == null || userIds.Count == 0)
        {
            throw new InvalidOperationException($"Can not create conversation without users");
        }

        var conversationName = name;
        if (string.IsNullOrEmpty(conversationName) && string.IsNullOrEmpty(entityId))
        {
            var users = await _communicationUserCrudService.GetAsync(userIds);
            var userNames = string.Join(", ", users.Select(x => x.UserName).ToArray());
            conversationName = $"Chat {userNames}";
        }

        var conversation = AbstractTypeFactory<Conversation>.TryCreateInstance();
        conversation.Id = Guid.NewGuid().ToString();
        conversation.Name = conversationName;
        conversation.IconUrl = iconUrl;
        conversation.EntityId = entityId;
        conversation.EntityType = entityType;
        conversation.LastMessageId = null;
        conversation.LastMessageTimestamp = DateTime.UtcNow;
        conversation.Users = new List<ConversationUser>();
        foreach (var userId in userIds)
        {
            var conversationUser = AbstractTypeFactory<ConversationUser>.TryCreateInstance();
            conversationUser.UserId = userId;
            conversationUser.ConversationId = conversation.Id;
            conversation.Users.Add(conversationUser);
        }

        await _conversationCrudService.SaveChangesAsync([conversation]);

        return conversation;
    }

    public virtual async Task<Conversation> GetOrCreateConversation(IList<string> userIds, string name, string iconUrl, string entityId, string entityType)
    {
        Conversation conversation = null;

        if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(entityType))
        {
            conversation = await GetConversationByEntity(entityId, entityType);
        }
        else
        {
            conversation = await GetConversationByUsers(userIds);
        }

        if (conversation == null)
        {
            conversation = await CreateConversation(userIds, name, iconUrl, entityId, entityType);
        }

        return conversation;
    }
}
