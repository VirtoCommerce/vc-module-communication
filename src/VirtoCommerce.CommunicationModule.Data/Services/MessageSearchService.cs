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
public class MessageSearchService : SearchService<SearchMessageCriteria,
    SearchMessageResult, Message, MessageEntity>,
    IMessageSearchService
{
    public MessageSearchService(
    Func<ICommunicationRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IMessageCrudService crudService,
    IOptions<CrudOptions> crudOptions)
    : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
    {
    }

    protected override IQueryable<MessageEntity> BuildQuery(IRepository repository, SearchMessageCriteria criteria)
    {
        var query = ((ICommunicationRepository)repository).Messages;

        //if (!string.IsNullOrEmpty(criteria.EntityId))
        //{
        //    query = query.Where(x => x.EntityId == criteria.EntityId);
        //}

        //if (!string.IsNullOrEmpty(criteria.EntityType))
        //{
        //    query = query.Where(x => x.EntityType == criteria.EntityType);
        //}

        if (!string.IsNullOrEmpty(criteria.ConversationId))
        {
            query = query.Where(x => x.ConversationId == criteria.ConversationId);
        }

        if (!string.IsNullOrEmpty(criteria.ThreadId))
        {
            query = query.Where(x => x.ThreadId == criteria.ThreadId);
        }
        else if (criteria.RootsOnly)
        {
            query = query.Where(x => string.IsNullOrEmpty(x.ThreadId));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(SearchMessageCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;
        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos = [
                    new SortInfo
                    {
                        SortColumn = ReflectionUtility.GetPropertyName<MessageEntity>(x => x.CreatedDate),
                        SortDirection = SortDirection.Ascending
                    }
                ];
        }

        return sortInfos;
    }

}
