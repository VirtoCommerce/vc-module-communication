using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface IMessageService
{
    Task<IList<Message>> GetMessagesByEntity(string entityId, string entityType);
    Task<IList<Message>> GetMessagesByThread(string threadId);
    //Task<IList<Message>> GetMessagesBySender(string senderId);

    Task SendMessage(Message message);
    Task UpdateMessage(Message message);
    Task DeleteMessage(IList<string> messageIds, bool withReplies);

    Task<Message> SetMessageReadStatus(string messageId, string recipientId, bool notRead);
    Task<Message> SetMessageReaction(string messageId, string userId, string reaction);
}
