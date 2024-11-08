using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class TestMessageSender : IMessageSender
{
    public string SenderName { get; } = nameof(TestMessageSender);

    public SettingDescriptor[] AvailableSettings { get; set; }

    public Task<SendMessageResult> SendMessage(Message message)
    {
        var sendMessageResult = new SendMessageResult { Status = "Success" };

        return Task.FromResult(sendMessageResult);
    }

    public virtual object Clone()
    {
        return MemberwiseClone();
    }
}
