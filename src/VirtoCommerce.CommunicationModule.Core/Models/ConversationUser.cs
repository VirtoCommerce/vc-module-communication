using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class ConversationUser : AuditableEntity, ICloneable
{
    public string ConversationId { get; set; }
    public string UserId { get; set; }

    #region ICloneable members
    public virtual object Clone()
    {
        return MemberwiseClone();
    }
    #endregion ICloneable members
}
