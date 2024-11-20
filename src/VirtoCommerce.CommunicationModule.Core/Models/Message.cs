using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class Message : AuditableEntity, ICloneable
{
    public string SenderId { get; set; }
    public string Content { get; set; }
    public string ThreadId { get; set; }
    public string ConversationId { get; set; }

    public IList<MessageAttachment> Attachments { get; set; }
    public IList<MessageRecipient> Recipients { get; set; }
    public IList<MessageReaction> Reactions { get; set; }
    public IList<Message> Answers { get; set; }

    public Conversation Conversation { get; set; }

    public virtual int? AnswersCount => Answers?.Count;
    public virtual string EntityId => Conversation?.EntityId;
    public virtual string EntityType => Conversation?.EntityType;

    #region ICloneable members
    public virtual object Clone()
    {
        return MemberwiseClone();
    }
    #endregion ICloneable members
}
