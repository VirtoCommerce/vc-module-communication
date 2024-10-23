using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Data.Services;
public class MessageSenderRegistrar : IMessageSenderRegistrar, IMessageSenderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public IEnumerable<IMessageSender> AllRegisteredSenders
    {
        get
        {
            return AbstractTypeFactory<IMessageSender>.AllTypeInfos.Select(x => AbstractTypeFactory<IMessageSender>.TryCreateInstance(x.TypeName));
        }
    }

    public MessageSenderRegistrar(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MessageSenderBuilder Register<TMessageSender>(Func<IMessageSender> factory = null) where TMessageSender : IMessageSender
    {
        var typeInfo = AbstractTypeFactory<IMessageSender>.RegisterType<TMessageSender>();
        var builder = new MessageSenderBuilder(_serviceProvider, typeof(TMessageSender), factory);
        typeInfo.WithFactory(() => builder.Build());
        return builder;
    }

    public IMessageSender Create(string typeName)
    {
        return AbstractTypeFactory<IMessageSender>.TryCreateInstance(typeName);
    }
}
