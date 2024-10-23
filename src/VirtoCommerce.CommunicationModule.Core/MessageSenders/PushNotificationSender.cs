using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Core.MessageSenders;
public class PushNotificationSender : IMessageSender
{
    private readonly IMessageCrudService _messageCrudService;

    public PushNotificationSender(
    IMessageCrudService messageCrudService
    )
    {
        _messageCrudService = messageCrudService;
    }

    public virtual string SenderName { get; } = nameof(PushNotificationSender);

    public virtual SettingDescriptor[] AvailableSettings { get; set; }

    public virtual Task<SendMessageResult> SendMessage(Message message)
    {
        var sendMessageResult = AbstractTypeFactory<SendMessageResult>.TryCreateInstance();
        sendMessageResult.Status = "Success";

        return Task.FromResult(sendMessageResult);
    }

    public virtual object Clone()
    {
        return MemberwiseClone();
    }
}
