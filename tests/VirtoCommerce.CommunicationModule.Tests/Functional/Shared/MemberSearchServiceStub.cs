using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.CustomerModule.Core.Services;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class MemberSearchServiceStub : IMemberSearchService
{
    public List<Member> Members = new List<Member>
    {
        new Organization
        {
            Id = "TestOrganizationUserId",
            MemberType = "Organization",
            Name = "TestOrganizationName",
            IconUrl = "organization-icon.url"
        },
        new Employee
        {
            Id = "TestEmployeeUserId",
            MemberType = "Employee",
            Name = "TestEmployeeName",
            IconUrl = "employee-icon.url"
        },
        new Contact
        {
            Id = "TestContactUserId",
            MemberType = "Contact",
            Name = "TestContactName",
            IconUrl = "contact-icon.url"
        },
    };

    public Task<MemberSearchResult> SearchMembersAsync(MembersSearchCriteria criteria)
    {
        var result = new MemberSearchResult();
        result.Results = Members.Where(x => criteria.ObjectIds.Contains(x.Id) && x.MemberType == criteria.MemberType).ToList();
        result.TotalCount = result.Results.Count;

        return Task.FromResult(result);
    }
}
