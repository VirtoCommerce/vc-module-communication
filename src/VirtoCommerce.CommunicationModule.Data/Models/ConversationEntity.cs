using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class ConversationEntity : AuditableEntity, IDataEntity<ConversationEntity, Conversation>
{
    public string Name { get; set; }

    [StringLength(2083)]
    public string IconUrl { get; set; }

    [StringLength(128)]
    public string EntityId { get; set; }

    [StringLength(128)]
    public string EntityType { get; set; }

    [StringLength(128)]
    public string LastMessageId { get; set; }

    public DateTime LastMessageTimestamp { get; set; }

    public virtual ObservableCollection<ConversationUserEntity> Users { get; set; }
    = new NullCollection<ConversationUserEntity>();

    public virtual ConversationEntity FromModel(Conversation model, PrimaryKeyResolvingMap pkMap)
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

        Name = model.Name;
        IconUrl = model.IconUrl;
        EntityId = model.EntityId;
        EntityType = model.EntityType;
        LastMessageId = model.LastMessageId;
        LastMessageTimestamp = model.LastMessageTimestamp;

        if (model.Users != null)
        {
            Users = new ObservableCollection<ConversationUserEntity>(model.Users.Select(x => AbstractTypeFactory<ConversationUserEntity>.TryCreateInstance().FromModel(x, pkMap)));
        }

        return this;
    }

    public virtual Conversation ToModel(Conversation model)
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

        model.Name = Name;
        model.IconUrl = IconUrl;
        model.EntityId = EntityId;
        model.EntityType = EntityType;
        model.LastMessageId = LastMessageId;
        model.LastMessageTimestamp = LastMessageTimestamp;

        if (Users != null && Users.Any())
        {
            model.Users = Users.Select(x => x.ToModel(AbstractTypeFactory<ConversationUser>.TryCreateInstance())).ToList();
        }

        return model;
    }

    public virtual void Patch(ConversationEntity target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        target.Name = Name;
        target.IconUrl = IconUrl;
        target.EntityId = EntityId;
        target.EntityType = EntityType;
        target.LastMessageId = LastMessageId;
        target.LastMessageTimestamp = LastMessageTimestamp;

        if (Users != null && Users.Any())
        {
            Users.Patch(target.Users, (source, target) => source.Patch(target));
        }
    }
}
