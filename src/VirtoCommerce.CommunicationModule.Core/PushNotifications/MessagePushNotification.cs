using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.CommunicationModule.Core.PushNotifications;
public class MessagePushNotification : PushNotification
{
    public MessagePushNotification(string creator)
    : base(creator)
    {
    }

    public string MessageId { get; set; }
    public string Content { get; set; }
    public string SenderId { get; set; }
}
