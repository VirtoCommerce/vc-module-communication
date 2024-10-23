using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public sealed class MessageSenderBuilder
{
    public MessageSenderBuilder(IServiceProvider serviceProvider, Type importerType, Func<IMessageSender> factory = null)
    {
        ServiceProvider = serviceProvider;
        MessageSender = factory != null ? factory() : Activator.CreateInstance(importerType) as IMessageSender;
    }

    public IMessageSender MessageSender { get; private set; }

    public IServiceProvider ServiceProvider { get; private set; }

    public MessageSenderBuilder WithSettings(IEnumerable<SettingDescriptor> settings)
    {
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }
        MessageSender.AvailableSettings = (MessageSender.AvailableSettings ?? Array.Empty<SettingDescriptor>()).Concat(settings).ToArray();
        if (ServiceProvider != null)
        {
            var settingsRegistrar = ServiceProvider.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(MessageSender.AvailableSettings);
            // TODO: need?
            //settingsRegistrar.RegisterSettingsForType(MessageSender.AvailableSettings, typeof(ImportProfile).Name);
        }
        return this;
    }

    public IMessageSender Build()
    {
        return MessageSender.Clone() as IMessageSender;
    }
}
