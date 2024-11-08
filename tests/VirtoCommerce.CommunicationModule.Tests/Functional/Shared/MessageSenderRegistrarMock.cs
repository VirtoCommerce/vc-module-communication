using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class MessageSenderRegistrarMock : IMessageSenderRegistrar
{
    private List<IMessageSender> _messageSenders;
    public IEnumerable<IMessageSender> AllRegisteredSenders { get => _messageSenders; }

    public MessageSenderBuilder Register<TMessageSender>(Func<IMessageSender> factory = null) where TMessageSender : IMessageSender
    {
        if (_messageSenders == null)
        {
            _messageSenders = new List<IMessageSender>();
        }
        _messageSenders.Add(factory());

        var typeInfo = AbstractTypeFactory<IMessageSender>.RegisterType<TMessageSender>();
        var builder = new MessageSenderBuilder(null, typeof(TMessageSender), factory);
        typeInfo.WithFactory(builder.Build);
        return builder;
    }
}
