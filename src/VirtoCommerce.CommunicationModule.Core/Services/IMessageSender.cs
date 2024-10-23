using System;
using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface IMessageSender : ICloneable
{
    string SenderName { get; }
    SettingDescriptor[] AvailableSettings { get; set; }
    Task<SendMessageResult> SendMessage(Message message);
}
