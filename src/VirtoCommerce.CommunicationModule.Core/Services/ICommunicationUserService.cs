using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface ICommunicationUserService
{
    Task<CommunicationUser> CreateCommunicationUser(string userId, string userType);
    Task<CommunicationUser> GetCommunicationUserByUserId(string userId, string userType);
    Task<IList<CommunicationUser>> SearchUsersByName(string userName, string userType);
}
