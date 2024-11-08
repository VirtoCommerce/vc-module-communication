using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class CommunicationUserServiceMock : ICommunicationUserService
{
    public Task<CommunicationUser> CreateCommunicationUser(string userId, string userType)
    {
        var communicationUser = new CommunicationUser
        {
            UserId = userId,
            UserName = "TestUserName",
            UserType = userType,
            Id = userId + userType
        };

        return Task.FromResult(communicationUser);
    }

    public Task<CommunicationUser> GetCommunicationUserByUserId(string userId, string userType)
    {
        return Task.FromResult(CreateCommunicationUser(userId, userType).Result);
    }

    public Task<CommunicationUser> GetOrCreateCommunicationUser(string userId, string userType)
    {
        var communicationUser = GetCommunicationUserByUserId(userId, userType).Result;
        if (communicationUser == null)
        {
            communicationUser = CreateCommunicationUser(userId, userType).Result;
        }

        return Task.FromResult(communicationUser);
    }

    public Task<IList<CommunicationUser>> SearchUsersByName(string userName, string userType)
    {
        throw new System.NotImplementedException();
    }
}
