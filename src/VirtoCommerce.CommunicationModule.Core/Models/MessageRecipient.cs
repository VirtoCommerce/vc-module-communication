using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class MessageRecipient : AuditableEntity
{
    public string MessageId { get; set; }
    public string RecipientId { get; set; }
    public string ReadStatus { get; set; }
    public DateTime ReadTimestamp { get; set; }

}
