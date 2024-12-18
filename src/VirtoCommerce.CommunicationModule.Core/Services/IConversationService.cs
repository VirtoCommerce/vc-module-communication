using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface IConversationService
{
    Task<Conversation> GetConversationByEntity(string entityId, string entityType);
    Task<Conversation> GetConversationByUsers(IList<string> userIds);
    Task<Conversation> CreateConversation(IList<string> userIds, string name, string iconUrl, string entityId, string entityType);
    Task<Conversation> GetOrCreateConversation(IList<string> userIds, string name, string iconUrl, string entityId, string entityType);
}
