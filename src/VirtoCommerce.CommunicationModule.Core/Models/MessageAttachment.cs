using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class MessageAttachment : AuditableEntity
{
    public string MessageId { get; set; }
    public string AttachmentUrl { get; set; }
    public string FileType { get; set; }
    public int FileSize { get; set; }
}
