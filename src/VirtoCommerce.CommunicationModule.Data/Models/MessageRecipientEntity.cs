using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class MessageRecipientEntity : AuditableEntity, IDataEntity<MessageRecipientEntity, MessageRecipient>
{
    [Required]
    [StringLength(128)]
    public string MessageId { get; set; }

    [Required]
    [StringLength(128)]
    public string RecipientId { get; set; }

    [Required]
    [StringLength(64)]
    public string ReadStatus { get; set; }

    public DateTime ReadTimestamp { get; set; }

    #region Navigation Properties
    public virtual MessageEntity Message { get; set; }
    public virtual CommunicationUserEntity Recipient { get; set; }
    #endregion Navigation Properties

    public virtual MessageRecipientEntity FromModel(MessageRecipient model, PrimaryKeyResolvingMap pkMap)
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
        RecipientId = model.RecipientId;
        ReadStatus = model.ReadStatus;
        ReadTimestamp = model.ReadTimestamp;

        return this;
    }

    public virtual MessageRecipient ToModel(MessageRecipient model)
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
        model.RecipientId = RecipientId;
        model.ReadStatus = ReadStatus;
        model.ReadTimestamp = ReadTimestamp;

        return model;
    }

    public virtual void Patch(MessageRecipientEntity target)
    {
        target.MessageId = MessageId;
        target.RecipientId = RecipientId;
        target.ReadStatus = ReadStatus;
        target.ReadTimestamp = ReadTimestamp;
    }
}
