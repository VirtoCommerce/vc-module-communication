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
    IQueryable<ConversationEntity> Conversations { get; }

    Task<IList<MessageEntity>> GetMessagesByIdsAsync(IList<string> ids, string responseGroup = null);
    Task<IList<MessageEntity>> GetMessagesByConversationAsync(string conversationId, string responseGroup = null);
    Task<IList<MessageEntity>> GetMessagesByThreadAsync(string threadId, string responseGroup = null);

    Task<IList<CommunicationUserEntity>> GetCommunicationUserByIdsAsync(IList<string> ids, string responseGroup = null);
    Task<CommunicationUserEntity> GetCommunicationUserByUserIdAsync(string userId, string userType);

    Task<IList<ConversationEntity>> GetConversationsByIdsAsync(IList<string> ids, string responseGroup = null);
    Task<ConversationEntity> GetConversationByEntityAsync(string entityId, string entityType);
    Task<ConversationEntity> GetConversationByUsersAsync(IList<string> userIds);
}
