using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models;
public class CommunicationUser : AuditableEntity, ICloneable
{
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string UserType { get; set; }
    public string AvatarUrl { get; set; }

    #region ICloneable members
    public virtual object Clone()
    {
        return MemberwiseClone();
    }
    #endregion ICloneable members

}
