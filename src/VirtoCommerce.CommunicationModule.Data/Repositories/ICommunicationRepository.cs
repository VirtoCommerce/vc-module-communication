using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Data.Repositories;
public interface ICommunicationRepository : IRepository
{
    IQueryable<MessageEntity> Messages { get; }
    IQueryable<CommunicationUserEntity> CommunicationUsers { get; }

    Task<IList<MessageEntity>> GetMessagesByIdsAsync(IList<string> ids, string responseGroup = null);
    Task<IList<MessageEntity>> GetMessagesByEntityAsync(string entityId, string entityType);

    Task<IList<CommunicationUserEntity>> GetCommunicationUserByIdsAsync(IList<string> ids, string responseGroup = null);
    Task<CommunicationUserEntity> GetCommunicationUserByUserIdAsync(string userId, string userType);
}
