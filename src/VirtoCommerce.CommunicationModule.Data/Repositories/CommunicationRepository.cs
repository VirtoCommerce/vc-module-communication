using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CommunicationModule.Data.Repositories;
public class CommunicationRepository : DbContextRepositoryBase<CommunicationDbContext>, ICommunicationRepository
{
    public CommunicationRepository(CommunicationDbContext dbContext)
    : base(dbContext)
    {
    }

    public IQueryable<MessageEntity> Messages => DbContext.Set<MessageEntity>();
    public IQueryable<CommunicationUserEntity> CommunicationUsers => DbContext.Set<CommunicationUserEntity>();
    public IQueryable<ConversationEntity> Conversations => DbContext.Set<ConversationEntity>();

    public virtual async Task<IList<MessageEntity>> GetMessagesByIdsAsync(IList<string> ids, string responseGroup = null)
    {
        var result = Array.Empty<MessageEntity>();
        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, MessageResponseGroup.WithoutAnswers);

        if (!ids.IsNullOrEmpty())
        {
            var query = Messages.Where(x => ids.Contains(x.Id));

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithAttachments))
            {
                query = query.Include(x => x.Attachments);
            }

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithRecipients))
            {
                query = query.Include(x => x.Recipients);
            }

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithReactions))
            {
                query = query.Include(x => x.Reactions);
            }

            result = await query.ToArrayAsync();
        }

        return result;
    }

    public virtual async Task<IList<MessageEntity>> GetMessagesByConversationAsync(string conversationId, string responseGroup = null)
    {
        var result = Array.Empty<MessageEntity>();
        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, MessageResponseGroup.WithoutAnswers);

        if (!string.IsNullOrEmpty(conversationId))
        {
            var query = Messages.Where(x => x.ConversationId == conversationId);

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithAttachments))
            {
                query = query.Include(x => x.Attachments);
            }

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithRecipients))
            {
                query = query.Include(x => x.Recipients);
            }

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithReactions))
            {
                query = query.Include(x => x.Reactions);
            }

            //query = query.Include(x => x.Conversation);

            result = await query.ToArrayAsync();
        }

        return result;
    }

    public virtual async Task<IList<MessageEntity>> GetMessagesByThreadAsync(string threadId, string responseGroup = null)
    {
        var result = Array.Empty<MessageEntity>();
        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, MessageResponseGroup.WithoutAnswers);
        if (!string.IsNullOrEmpty(threadId))
        {
            var query = Messages.Where(x => x.ThreadId == threadId);

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithAttachments))
            {
                query = query.Include(x => x.Attachments);
            }

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithRecipients))
            {
                query = query.Include(x => x.Recipients);
            }

            if (respGroupEnum.HasFlag(MessageResponseGroup.WithReactions))
            {
                query = query.Include(x => x.Reactions);
            }

            result = await query.ToArrayAsync();
        }

        return result;
    }

    public virtual async Task<IList<CommunicationUserEntity>> GetCommunicationUserByIdsAsync(IList<string> ids, string responseGroup = null)
    {
        var result = Array.Empty<CommunicationUserEntity>();

        if (!ids.IsNullOrEmpty())
        {
            result = await CommunicationUsers.Where(x => ids.Contains(x.Id)).ToArrayAsync();
        }

        return result;
    }

    public virtual async Task<CommunicationUserEntity> GetCommunicationUserByUserIdAsync(string userId, string userType)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            return await CommunicationUsers.FirstOrDefaultAsync(x => x.UserId == userId && x.UserType == userType);
        }

        return null;
    }

    public virtual async Task<IList<ConversationEntity>> GetConversationsByIdsAsync(IList<string> ids, string responseGroup = null)
    {
        var result = Array.Empty<ConversationEntity>();
        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, ConversationResponseGroup.None);

        if (!ids.IsNullOrEmpty())
        {
            result = await Conversations.Where(x => ids.Contains(x.Id)).Include(x => x.Users).ToArrayAsync();
        }
        return result;
    }

    public virtual async Task<ConversationEntity> GetConversationByEntityAsync(string entityId, string entityType)
    {
        ConversationEntity result = null;

        if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(entityType))
        {
            result = await Conversations.FirstOrDefaultAsync(x => x.EntityId == entityId && x.EntityType == entityType);
        }

        return result;
    }

    public virtual async Task<ConversationEntity> GetConversationByUsersAsync(IList<string> userIds)
    {
        ConversationEntity result = null;

        if (userIds != null && userIds.Any())
        {
            result = await Conversations.Include(x => x.Users).FirstOrDefaultAsync(x => x.EntityId == null && x.Users.Select(u => u.UserId).Intersect(userIds).Count() == userIds.Count);
        }

        return result;
    }
}
