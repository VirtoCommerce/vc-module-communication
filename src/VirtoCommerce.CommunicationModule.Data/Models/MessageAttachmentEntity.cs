using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class MessageAttachmentEntity : AuditableEntity, IDataEntity<MessageAttachmentEntity, MessageAttachment>
{
    [Required]
    [StringLength(128)]
    public string MessageId { get; set; }

    [StringLength(2083)]
    public string AttachmentUrl { get; set; }

    [StringLength(256)]
    public string FileName { get; set; }

    [StringLength(64)]
    public string FileType { get; set; }

    public int FileSize { get; set; }

    #region Navigation Properties
    public virtual MessageEntity Message { get; set; }
    #endregion Navigation Properties

    public virtual MessageAttachmentEntity FromModel(MessageAttachment model, PrimaryKeyResolvingMap pkMap)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        MessageId = model.MessageId;
        AttachmentUrl = model.AttachmentUrl;
        FileName = model.FileName;
        FileType = model.FileType;
        FileSize = model.FileSize;

        return this;
    }

    public virtual MessageAttachment ToModel(MessageAttachment model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.MessageId = MessageId;
        model.AttachmentUrl = AttachmentUrl;
        model.FileName = FileName;
        model.FileType = FileType;
        model.FileSize = FileSize;

        return model;
    }

    public virtual void Patch(MessageAttachmentEntity target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        target.MessageId = MessageId;
        target.AttachmentUrl = AttachmentUrl;
        target.FileName = FileName;
        target.FileType = FileType;
        target.FileSize = FileSize;
    }
}
