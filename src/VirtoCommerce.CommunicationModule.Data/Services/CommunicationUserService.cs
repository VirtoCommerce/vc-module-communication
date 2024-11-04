using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security.Search;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class CommunicationUserService : ICommunicationUserService
{
    private readonly Func<ICommunicationRepository> _repositoryFactory;
    private readonly ICommunicationUserCrudService _communicationUserCrudService;
    private readonly IMemberSearchService _memberSearchService;
    private readonly IUserSearchService _userSearchService;

    public CommunicationUserService(
        Func<ICommunicationRepository> repositoryFactory,
        ICommunicationUserCrudService communicationUserCrudService,
        IMemberSearchService memberSearchService,
        IUserSearchService userSearchService
        )
    {
        _repositoryFactory = repositoryFactory;
        _communicationUserCrudService = communicationUserCrudService;
        _memberSearchService = memberSearchService;
        _userSearchService = userSearchService;
    }

    public virtual async Task<CommunicationUser> CreateCommunicationUser(string userId, string userType)
    {
        var communicationUser = AbstractTypeFactory<CommunicationUser>.TryCreateInstance();
        communicationUser.UserId = userId;
        communicationUser.UserType = userType;

        var membersSearchCriteria = AbstractTypeFactory<MembersSearchCriteria>.TryCreateInstance();
        membersSearchCriteria.ObjectIds = [userId];

        switch (userType)
        {
            case ModuleConstants.CommunicationUserType.Organization:
                membersSearchCriteria.MemberType = typeof(Organization).Name;
                break;

            case ModuleConstants.CommunicationUserType.Employee:
                membersSearchCriteria.MemberType = typeof(Employee).Name;
                break;

            case ModuleConstants.CommunicationUserType.Customer:
                membersSearchCriteria.MemberType = typeof(Contact).Name;
                break;
        }

        var member = (await _memberSearchService.SearchMembersAsync(membersSearchCriteria)).Results.FirstOrDefault();

        communicationUser.UserName = member?.Name ?? userId;
        communicationUser.AvatarUrl = member?.IconUrl;

        await _communicationUserCrudService.SaveChangesAsync([communicationUser]);

        return communicationUser;
    }

    public virtual async Task<CommunicationUser> GetCommunicationUserByUserId(string userId, string userType)
    {
        using var repository = _repositoryFactory();

        var communicationUserEntitiy = await repository.GetCommunicationUserByUserIdAsync(userId, userType);

        if (communicationUserEntitiy != null)
        {
            return communicationUserEntitiy.ToModel(AbstractTypeFactory<CommunicationUser>.TryCreateInstance());
        }

        return null;
    }

    public virtual async Task<CommunicationUser> GetOrCreateCommunicationUser(string userId, string userType)
    {
        var communicationUser = await GetCommunicationUserByUserId(userId, userType);
        if (communicationUser == null)
        {
            communicationUser = await CreateCommunicationUser(userId, userType);
        }

        return communicationUser;
    }

    public virtual Task<IList<CommunicationUser>> SearchUsersByName(string userName, string userType)
    {
        throw new NotImplementedException();
    }
}
