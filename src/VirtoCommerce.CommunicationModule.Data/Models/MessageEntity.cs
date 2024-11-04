using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.CommunicationModule.Data.Models;
public class MessageEntity : AuditableEntity, IDataEntity<MessageEntity, Message>
{
    [Required]
    [StringLength(128)]
    public string SenderId { get; set; }

    [Required]
    [StringLength(128)]
    public string EntityId { get; set; }

    [Required]
    [StringLength(64)]
    public string EntityType { get; set; }

    [Required]
    public string Content { get; set; }

    [StringLength(128)]
    public string ThreadId { get; set; }

    #region Navigation Properties
    public virtual CommunicationUserEntity Sender { get; set; }

    public virtual ObservableCollection<MessageAttachmentEntity> Attachments { get; set; }
        = new NullCollection<MessageAttachmentEntity>();

    public virtual ObservableCollection<MessageRecipientEntity> Recipients { get; set; }
        = new NullCollection<MessageRecipientEntity>();

    public virtual ObservableCollection<MessageReactionEntity> Reactions { get; set; }
        = new NullCollection<MessageReactionEntity>();
    #endregion Navigation Properties

    public virtual MessageEntity FromModel(Message model, PrimaryKeyResolvingMap pkMap)
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

        SenderId = model.SenderId;
        EntityId = model.EntityId;
        EntityType = model.EntityType;
        Content = model.Content;
        ThreadId = model.ThreadId;

        if (model.Attachments != null)
        {
            Attachments = new ObservableCollection<MessageAttachmentEntity>(model.Attachments.Select(x => AbstractTypeFactory<MessageAttachmentEntity>.TryCreateInstance().FromModel(x, pkMap)));
        }

        if (model.Recipients != null)
        {
            Recipients = new ObservableCollection<MessageRecipientEntity>(model.Recipients.Select(x => AbstractTypeFactory<MessageRecipientEntity>.TryCreateInstance().FromModel(x, pkMap)));
        }

        if (model.Reactions != null)
        {
            Reactions = new ObservableCollection<MessageReactionEntity>(model.Reactions.Select(x => AbstractTypeFactory<MessageReactionEntity>.TryCreateInstance().FromModel(x, pkMap)));
        }

        return this;
    }

    public virtual Message ToModel(Message model)
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

        model.SenderId = SenderId;
        model.EntityId = EntityId;
        model.EntityType = EntityType;
        model.Content = Content;
        model.ThreadId = ThreadId;

        if (Attachments != null && Attachments.Any())
        {
            model.Attachments = Attachments.Select(x => x.ToModel(AbstractTypeFactory<MessageAttachment>.TryCreateInstance())).ToList();
        }

        if (Recipients != null && Recipients.Any())
        {
            model.Recipients = Recipients.Select(x => x.ToModel(AbstractTypeFactory<MessageRecipient>.TryCreateInstance())).ToList();
        }

        if (Reactions != null && Reactions.Any())
        {
            model.Reactions = Reactions.Select(x => x.ToModel(AbstractTypeFactory<MessageReaction>.TryCreateInstance())).ToList();
        }

        return model;
    }

    public virtual void Patch(MessageEntity target)
    {
        target.SenderId = SenderId;
        target.EntityId = EntityId;
        target.EntityType = EntityType;
        target.Content = Content;
        target.ThreadId = ThreadId;

        if (Attachments != null && Attachments.Any())
        {
            Attachments.Patch(target.Attachments, (source, target) => source.Patch(target));
        }

        if (Recipients != null && Recipients.Any())
        {
            Recipients.Patch(target.Recipients, (source, target) => source.Patch(target));
        }

        if (Reactions != null && Reactions.Any())
        {
            Reactions.Patch(target.Reactions, (source, target) => source.Patch(target));
        }
    }
}
