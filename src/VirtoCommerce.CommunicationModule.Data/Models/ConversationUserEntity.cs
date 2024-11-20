using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class ConversationUserEntity : AuditableEntity, IDataEntity<ConversationUserEntity, ConversationUser>
{
    [Required]
    [StringLength(128)]
    public string ConversationId { get; set; }

    [Required]
    [StringLength(128)]
    public string UserId { get; set; }

    #region Navigation Properties
    public virtual ConversationEntity Conversation { get; set; }
    #endregion Navigation Properties

    public virtual ConversationUserEntity FromModel(ConversationUser model, PrimaryKeyResolvingMap pkMap)
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

        ConversationId = model.ConversationId;
        UserId = model.UserId;

        return this;
    }

    public virtual ConversationUser ToModel(ConversationUser model)
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

        model.ConversationId = ConversationId;
        model.UserId = UserId;

        return model;
    }

    public virtual void Patch(ConversationUserEntity target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        target.ConversationId = ConversationId;
        target.UserId = UserId;
    }
}
