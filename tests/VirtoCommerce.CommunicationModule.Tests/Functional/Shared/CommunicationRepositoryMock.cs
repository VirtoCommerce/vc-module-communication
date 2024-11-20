using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
class CommunicationRepositoryMock : ICommunicationRepository
{
    public IQueryable<MessageEntity> Messages
    {
        get
        {
            return MessageEntities.AsAsyncQueryable();
        }
    }
    public List<MessageEntity> MessageEntities = new();

    public IQueryable<CommunicationUserEntity> CommunicationUsers => throw new System.NotImplementedException();
    public List<CommunicationUserEntity> CommunicationUserEntities = new();

    public IQueryable<ConversationEntity> Conversations => throw new System.NotImplementedException();
    public List<ConversationEntity> ConversationEntities = new();

    public IUnitOfWork UnitOfWork => new Mock<IUnitOfWork>().Object;

    public void Add<T>(T item) where T : class
    {
        if (item.GetType() == typeof(MessageEntity))
        {
            var messageEntity = item as MessageEntity;
            if (string.IsNullOrEmpty(messageEntity.Id))
            {
                messageEntity.Id = nameof(MessageEntity) + messageEntity.SenderId + messageEntity.CreatedDate.Ticks.ToString();
            }
            MessageEntities.Add(messageEntity);
        }
        else if (item.GetType() == typeof(CommunicationUserEntity))
        {
            var communicationUserEntity = item as CommunicationUserEntity;
            if (string.IsNullOrEmpty(communicationUserEntity.Id))
            {
                communicationUserEntity.Id = nameof(CommunicationUserEntity) + communicationUserEntity.UserName;
            }
            CommunicationUserEntities.Add(communicationUserEntity);
        }
        else if (item.GetType() == typeof(ConversationEntity))
        {
            var conversationEntity = item as ConversationEntity;
            if (string.IsNullOrEmpty(conversationEntity.Id))
            {
                conversationEntity.Id = nameof(ConversationEntity) + conversationEntity.Name;
            }
            ConversationEntities.Add(conversationEntity);
        }
    }

    public void Attach<T>(T item) where T : class
    {
        throw new System.NotImplementedException();
    }

    public void Remove<T>(T item) where T : class
    {
        if (item.GetType() == typeof(MessageEntity))
        {
            var messageEntity = item as MessageEntity;
            MessageEntities.Remove(messageEntity);
        }
        else if (item.GetType() == typeof(CommunicationUserEntity))
        {
            var communicationUserEntity = item as CommunicationUserEntity;
            CommunicationUserEntities.Remove(communicationUserEntity);
        }
        else if (item.GetType() == typeof(ConversationEntity))
        {
            var conversationEntity = item as ConversationEntity;
            ConversationEntities.Remove(conversationEntity);
        }
    }

    public void Update<T>(T item) where T : class
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
    }

    public Task<IList<CommunicationUserEntity>> GetCommunicationUserByIdsAsync(IList<string> ids, string responseGroup = null)
    {
        var result = new List<CommunicationUserEntity>();
        if (!ids.IsNullOrEmpty())
        {
            result = CommunicationUserEntities.Where(x => ids.Contains(x.Id)).ToList();
        }
        return Task.FromResult(result as IList<CommunicationUserEntity>);
    }

    public Task<CommunicationUserEntity> GetCommunicationUserByUserIdAsync(string userId, string userType)
    {
        CommunicationUserEntity result = null;
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userType))
        {
            result = CommunicationUserEntities.FirstOrDefault(x => x.UserId == userId && x.UserType == userType);
        }
        return Task.FromResult(result);
    }

    public Task<IList<MessageEntity>> GetMessagesByConversationAsync(string conversationId, string responseGroup = null)
    {
        var result = new List<MessageEntity>();
        if (!string.IsNullOrEmpty(conversationId))
        {
            result = MessageEntities.Where(x => x.ConversationId == conversationId).ToList();
        }
        return Task.FromResult(result as IList<MessageEntity>);
    }

    public Task<IList<MessageEntity>> GetMessagesByIdsAsync(IList<string> ids, string responseGroup = null)
    {
        var result = new List<MessageEntity>();
        if (!ids.IsNullOrEmpty())
        {
            result = MessageEntities.Where(x => ids.Contains(x.Id)).ToList();
        }
        return Task.FromResult(result as IList<MessageEntity>);
    }

    public Task<IList<MessageEntity>> GetMessagesByThreadAsync(string threadId, string responseGroup = null)
    {
        var result = new List<MessageEntity>();
        if (!string.IsNullOrEmpty(threadId))
        {
            result = MessageEntities.Where(x => x.ThreadId == threadId).ToList();
        }
        return Task.FromResult(result as IList<MessageEntity>);
    }

    public Task<IList<ConversationEntity>> GetConversationsByIdsAsync(IList<string> ids, string responseGroup = null)
    {
        var result = new List<ConversationEntity>();

        if (!ids.IsNullOrEmpty())
        {
            result = ConversationEntities.Where(x => ids.Contains(x.Id)).ToList();
        }

        return Task.FromResult(result as IList<ConversationEntity>);
    }

    public Task<ConversationEntity> GetConversationByEntityAsync(string entityId, string entityType)
    {
        ConversationEntity result = null;

        if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(entityType))
        {
            result = ConversationEntities.FirstOrDefault(x => x.EntityId == entityId && x.EntityType == entityId);
        }

        return Task.FromResult(result);
    }

    public Task<ConversationEntity> GetConversationByUsersAsync(IList<string> userIds)
    {
        throw new System.NotImplementedException();
    }
}
