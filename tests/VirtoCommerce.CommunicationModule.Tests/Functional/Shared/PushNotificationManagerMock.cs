using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class PushNotificationManagerMock : IPushNotificationManager
{
    public List<PushNotification> Notifications = new();
    public PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria)
    {
        throw new System.NotImplementedException();
    }

    public void Send(PushNotification notification)
    {
        Notifications.Add(notification);
    }

    public Task SendAsync(PushNotification notification)
    {
        Notifications.Add(notification);
        return Task.CompletedTask;
    }
}
