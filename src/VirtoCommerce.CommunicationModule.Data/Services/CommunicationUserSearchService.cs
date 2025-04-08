using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Models.Search;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class CommunicationUserSearchService : SearchService<SearchCommunicationUserCriteria,
    SearchCommunicationUserResult, CommunicationUser, CommunicationUserEntity>,
    ICommunicationUserSearchService
{
    public CommunicationUserSearchService(
    Func<ICommunicationRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    ICommunicationUserCrudService crudService,
    IOptions<CrudOptions> crudOptions
    )
    : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
    {
    }

    protected override IQueryable<CommunicationUserEntity> BuildQuery(IRepository repository, SearchCommunicationUserCriteria criteria)
    {
        var query = ((ICommunicationRepository)repository).CommunicationUsers;

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(SearchCommunicationUserCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;
        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos = [
                    new SortInfo
                    {
                        SortColumn = ReflectionUtility.GetPropertyName<CommunicationUserEntity>(x => x.CreatedDate),
                        SortDirection = SortDirection.Descending
                    }
                ];
        }

        return sortInfos;
    }
}
