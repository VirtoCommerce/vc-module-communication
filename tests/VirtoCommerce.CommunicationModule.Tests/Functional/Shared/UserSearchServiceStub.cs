using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class UserSearchServiceStub : IUserSearchService
{
    public List<ApplicationUser> Users = new List<ApplicationUser>
    {
        new ApplicationUser
        {
            Id = "TestApplicationUserId",
            MemberId = "TestUserId",
            UserName = "TestUserName"
        }
    };

    public Task<UserSearchResult> SearchUsersAsync(UserSearchCriteria criteria)
    {
        var result = new UserSearchResult();
        result.Results = Users.Where(x => x.MemberId == criteria.MemberId).ToList();
        result.TotalCount = result.Results.Count;

        return Task.FromResult(result);
    }
}
