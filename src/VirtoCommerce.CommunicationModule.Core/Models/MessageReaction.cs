using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class MessageReaction : AuditableEntity
{
    public string MessageId { get; set; }

    public string UserId { get; set; }

    public string Reaction { get; set; }
}
