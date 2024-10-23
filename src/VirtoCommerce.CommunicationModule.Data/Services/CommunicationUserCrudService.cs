using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class CommunicationUserCrudService : CrudService<CommunicationUser, CommunicationUserEntity,
    GenericChangedEntryEvent<CommunicationUser>, GenericChangedEntryEvent<CommunicationUser>>,
    ICommunicationUserCrudService
{
    private readonly Func<ICommunicationRepository> _repositoryFactory;

    public CommunicationUserCrudService(
        Func<ICommunicationRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IEventPublisher eventPublisher
        ) : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        _repositoryFactory = repositoryFactory;
    }

    protected override Task<IList<CommunicationUserEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((ICommunicationRepository)repository).GetCommunicationUserByIdsAsync(ids, responseGroup);
    }
}
