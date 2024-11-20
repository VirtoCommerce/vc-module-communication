using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class Conversation : AuditableEntity, ICloneable
{
    public string Name { get; set; }
    public string IconUrl { get; set; }
    public string EntityId { get; set; }
    public string EntityType { get; set; }
    public string LastMessageId { get; set; }
    public DateTime LastMessageTimestamp { get; set; }

    public IList<ConversationUser> Users { get; set; }
    public int UnreadMessagesCount { get; set; }
    public virtual Message LastMessage { get; set; }

    #region ICloneable members
    public virtual object Clone()
    {
        return MemberwiseClone();
    }
    #endregion ICloneable members
}
