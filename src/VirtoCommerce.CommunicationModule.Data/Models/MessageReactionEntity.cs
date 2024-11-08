using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class MessageReactionEntity : AuditableEntity, IDataEntity<MessageReactionEntity, MessageReaction>
{
    [Required]
    [StringLength(128)]
    public string MessageId { get; set; }

    [Required]
    [StringLength(128)]
    public string UserId { get; set; }

    [Required]
    public string Reaction { get; set; }

    #region Navigation Properties
    public virtual MessageEntity Message { get; set; }
    public virtual CommunicationUserEntity User { get; set; }
    #endregion Navigation Properties

    public virtual MessageReactionEntity FromModel(MessageReaction model, PrimaryKeyResolvingMap pkMap)
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
        UserId = model.UserId;
        Reaction = model.Reaction;

        return this;
    }

    public virtual MessageReaction ToModel(MessageReaction model)
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
        model.UserId = UserId;
        model.Reaction = Reaction;

        return model;
    }

    public virtual void Patch(MessageReactionEntity target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        target.MessageId = MessageId;
        target.UserId = UserId;
        target.Reaction = Reaction;
    }
}
