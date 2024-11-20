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
public class ConversationCrudService : CrudService<Conversation, ConversationEntity,
    GenericChangedEntryEvent<Conversation>, GenericChangedEntryEvent<Conversation>>,
    IConversationCrudService
{
    private readonly Func<ICommunicationRepository> _repositoryFactory;

    public ConversationCrudService(
        Func<ICommunicationRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IEventPublisher eventPublisher
        ) : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        _repositoryFactory = repositoryFactory;
    }

    protected override Task<IList<ConversationEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((ICommunicationRepository)repository).GetConversationsByIdsAsync(ids, responseGroup);
    }

}
