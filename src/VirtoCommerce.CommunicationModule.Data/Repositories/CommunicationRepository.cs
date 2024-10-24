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

    public virtual async Task<IList<MessageEntity>> GetMessagesByEntityAsync(string entityId, string entityType, string responseGroup = null)
    {
        var result = Array.Empty<MessageEntity>();
        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, MessageResponseGroup.WithoutAnswers);

        if (!string.IsNullOrEmpty(entityId))
        {
            var query = Messages.Where(x => x.EntityId == entityId && x.EntityType == entityType);

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
}
