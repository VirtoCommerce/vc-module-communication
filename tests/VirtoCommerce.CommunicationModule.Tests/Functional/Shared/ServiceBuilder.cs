using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class ServiceBuilder
{
    public ServiceCollection GetServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddOptions();
        services.AddTransient<ILoggerFactory, LoggerFactory>();
        services.AddTransient<ICommunicationRepository, CommunicationRepositoryMock>();
        services.AddTransient<ISettingsManager, SettingsManagerMock>();
        services.AddTransient<ICommunicationUserCrudService, CommunicationUserCrudServiceMock>();
        services.AddTransient<ICommunicationUserService, CommunicationUserServiceMock>();
        services.AddTransient<IMessageCrudService, MessageCrudServiceMock>();
        services.AddTransient<IMessageSenderRegistrar, MessageSenderRegistrarMock>();
        services.AddTransient<IMessageSender, TestMessageSender>();
        services.AddTransient<IConversationCrudService, ConversationCrudServiceMock>();

        services.AddTransient<IUserSearchService, UserSearchServiceStub>();
        services.AddTransient<IMemberSearchService, MemberSearchServiceStub>();
        services.AddTransient<IPushNotificationManager, PushNotificationManagerMock>();
        services.AddTransient<IOptions<CrudOptions>, CrudOptionsMock>();

        return services;
    }
}
