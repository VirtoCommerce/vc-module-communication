using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.PushNotifications;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Core.MessageSenders;
public class PushNotificationSender : IMessageSender
{
    private readonly ICommunicationUserCrudService _communicationUserCrudService;
    private readonly IUserSearchService _userSearchService;
    private readonly IPushNotificationManager _pushNotificationManager;

    public PushNotificationSender(
        ICommunicationUserCrudService communicationUserCrudService,
        IUserSearchService userSearchService,
        IPushNotificationManager pushNotificationManager
        )
    {
        _communicationUserCrudService = communicationUserCrudService;
        _userSearchService = userSearchService;
        _pushNotificationManager = pushNotificationManager;
    }

    public virtual string SenderName { get; } = nameof(PushNotificationSender);

    public virtual SettingDescriptor[] AvailableSettings { get; set; }

    public virtual async Task<SendMessageResult> SendMessage(Message message)
    {
        var recipientIds = message.Recipients?.Select(x => x.RecipientId);
        if (recipientIds != null && recipientIds.Any())
        {
            foreach (var recipientId in recipientIds)
            {
                var recipient = await _communicationUserCrudService.GetNoCloneAsync(recipientId);
                var notificationRecipient = recipient.UserId;
                if (recipient.UserType == ModuleConstants.CommunicationUserType.Employee || recipient.UserType == ModuleConstants.CommunicationUserType.Customer)
                {
                    var user = (await _userSearchService.SearchUsersAsync(new UserSearchCriteria { MemberId = recipient.UserId })).Results.FirstOrDefault();
                    if (user != null)
                    {
                        notificationRecipient = user.UserName;
                    }
                }

                var pushNotification = new MessagePushNotification(notificationRecipient)
                {
                    Title = "New message",
                    MessageId = message.Id,
                    ThreadId = message.ThreadId,
                    Content = message.Content,
                    SenderId = message.SenderId,
                    EntityId = message.EntityId,
                    EntityType = message.EntityType,
                };

                await _pushNotificationManager.SendAsync(pushNotification);
            }
        }

        var sendMessageResult = AbstractTypeFactory<SendMessageResult>.TryCreateInstance();
        sendMessageResult.Status = "Success";

        return sendMessageResult;
    }

    public virtual object Clone()
    {
        return MemberwiseClone();
    }
}
