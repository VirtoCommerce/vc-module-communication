using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
public class ConversationSearchService : SearchService<SearchConversationCriteria,
    SearchConversationResult, Conversation, ConversationEntity>,
    IConversationSearchService
{
    public ConversationSearchService(
        Func<ICommunicationRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IConversationCrudService crudService,
        IOptions<CrudOptions> crudOptions)
        : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
    {
    }

    protected override IQueryable<ConversationEntity> BuildQuery(IRepository repository, SearchConversationCriteria criteria)
    {
        var query = ((ICommunicationRepository)repository).Conversations;

        if (criteria.UserIds != null && criteria.UserIds.Any())
        {
            query = query.Include(x => x.Users);
            query = query.Where(x => x.Users.Select(u => u.UserId).Intersect(criteria.UserIds).Count() > 0);
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(SearchConversationCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;
        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos = [
                    new SortInfo
                    {
                        SortColumn = ReflectionUtility.GetPropertyName<ConversationEntity>(x => x.LastMessageTimestamp),
                        SortDirection = SortDirection.Descending
                    }
                ];
        }

        return sortInfos;
    }
}
