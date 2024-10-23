using System;
using System.Collections.Generic;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface IMessageSenderRegistrar
{
    IEnumerable<IMessageSender> AllRegisteredSenders { get; }
    MessageSenderBuilder Register<TMessageSender>(Func<IMessageSender> factory = null) where TMessageSender : IMessageSender;
}
