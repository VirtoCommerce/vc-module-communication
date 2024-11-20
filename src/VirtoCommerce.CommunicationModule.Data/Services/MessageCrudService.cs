using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly Func<ICommunicationRepository> _repositoryFactory;
    private readonly ICommunicationUserCrudService _communicationUserCrudService;

    public MessageCrudService(
        Func<ICommunicationRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IEventPublisher eventPublisher,
        ICommunicationUserCrudService communicationUserCrudService
        ) : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        _repositoryFactory = repositoryFactory;
        _communicationUserCrudService = communicationUserCrudService;
    }

    protected override Task<IList<MessageEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((ICommunicationRepository)repository).GetMessagesByIdsAsync(ids, responseGroup);
    }

    public override async Task<IList<Message>> GetAsync(IList<string> ids, string responseGroup = null, bool clone = true)
    {
        var messages = await base.GetAsync(ids, responseGroup, clone);

        var respGroupEnum = EnumUtility.SafeParseFlags(responseGroup, MessageResponseGroup.WithoutAnswers);

        if (respGroupEnum.HasFlag(MessageResponseGroup.WithSender))
        {
            if (!messages.IsNullOrEmpty())
            {
                var sendersIds = messages.Select(x => x.SenderId).ToList();
                var senders = await _communicationUserCrudService.GetAsync(sendersIds);
                foreach (var message in messages)
                {
                    message.Sender = senders.FirstOrDefault(x => x.Id == message.SenderId);
                }
            }
        }

        if (respGroupEnum.HasFlag(MessageResponseGroup.WithAnswers))
        {
            if (!messages.IsNullOrEmpty())
            {
                using var repository = _repositoryFactory();
                foreach (var message in messages)
                {
                    var answers = await repository.GetMessagesByThreadAsync(message.Id, MessageResponseGroup.None.ToString());
                    message.Answers = answers.Select(x => x.ToModel(AbstractTypeFactory<Message>.TryCreateInstance())).ToList();
                }
            }
        }

        return messages;
    }
}
