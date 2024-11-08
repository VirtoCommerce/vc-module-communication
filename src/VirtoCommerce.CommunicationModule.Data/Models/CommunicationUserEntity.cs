using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class CommunicationUserEntity : AuditableEntity, IDataEntity<CommunicationUserEntity, CommunicationUser>
{
    [Required]
    [StringLength(255)]
    public string UserName { get; set; }

    [Required]
    [StringLength(128)]
    public string UserId { get; set; }

    [Required]
    [StringLength(64)]
    public string UserType { get; set; }

    [StringLength(2083)]
    public string AvatarUrl { get; set; }

    public virtual CommunicationUserEntity FromModel(CommunicationUser model, PrimaryKeyResolvingMap pkMap)
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

        UserName = model.UserName;
        UserId = model.UserId;
        UserType = model.UserType;
        AvatarUrl = model.AvatarUrl;

        return this;
    }

    public virtual CommunicationUser ToModel(CommunicationUser model)
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

        model.UserName = UserName;
        model.UserId = UserId;
        model.UserType = UserType;
        model.AvatarUrl = AvatarUrl;

        return model;
    }

    public virtual void Patch(CommunicationUserEntity target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        target.UserName = UserName;
        target.UserId = UserId;
        target.UserType = UserType;
        target.AvatarUrl = AvatarUrl;
    }
}
