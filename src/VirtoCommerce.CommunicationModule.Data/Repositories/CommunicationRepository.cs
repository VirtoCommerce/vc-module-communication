using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        if (!ids.IsNullOrEmpty())
        {
            result = await Messages.Where(x => ids.Contains(x.Id)).Include(x => x.Attachments).Include(x => x.Recipients).Include(x => x.Reactions).ToArrayAsync();
        }

        return result;
    }

    public virtual async Task<IList<MessageEntity>> GetMessagesByEntityAsync(string entityId, string entityType)
    {
        var result = Array.Empty<MessageEntity>();

        if (!string.IsNullOrEmpty(entityId))
        {
            result = await Messages.Where(x => x.EntityId == entityId && x.EntityType == entityType).Include(x => x.Attachments).Include(x => x.Recipients).Include(x => x.Reactions).ToArrayAsync();
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
