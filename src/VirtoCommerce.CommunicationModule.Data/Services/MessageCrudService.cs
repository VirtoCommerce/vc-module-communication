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
public class MessageCrudService : CrudService<Message, MessageEntity,
    GenericChangedEntryEvent<Message>, GenericChangedEntryEvent<Message>>,
    IMessageCrudService
{
    public MessageCrudService(
    Func<ICommunicationRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher
    ) : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {

    }

    protected override Task<IList<MessageEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((ICommunicationRepository)repository).GetMessagesByIdsAsync(ids, responseGroup);
    }
}
