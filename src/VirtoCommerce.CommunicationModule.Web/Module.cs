using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CommunicationModule.Core;
using VirtoCommerce.CommunicationModule.Core.MessageSenders;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data;
using VirtoCommerce.CommunicationModule.Data.ExportImport;
using VirtoCommerce.CommunicationModule.Data.Handlers;
using VirtoCommerce.CommunicationModule.Data.MySql;
using VirtoCommerce.CommunicationModule.Data.PostgreSql;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.CommunicationModule.Data.Services;
using VirtoCommerce.CommunicationModule.Data.SqlServer;
using VirtoCommerce.CustomerModule.Core.Events;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Web;

public class Module : IModule, IHasConfiguration, IExportSupport, IImportSupport
{
    private IApplicationBuilder _appBuilder;

    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<CommunicationDbContext>(options =>
        {
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

            switch (databaseProvider)
            {
                case "MySql":
                    options.UseMySqlDatabase(connectionString);
                    break;
                case "PostgreSql":
                    options.UsePostgreSqlDatabase(connectionString);
                    break;
                default:
                    options.UseSqlServerDatabase(connectionString);
                    break;
            }
        });

        serviceCollection.AddTransient<ICommunicationRepository, CommunicationRepository>();
        serviceCollection.AddTransient<Func<ICommunicationRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<ICommunicationRepository>());

        serviceCollection.AddSingleton<MessageSenderRegistrar>();
        serviceCollection.AddSingleton<IMessageSenderFactory>(provider => provider.GetService<MessageSenderRegistrar>());
        serviceCollection.AddSingleton<IMessageSenderRegistrar>(provider => provider.GetService<MessageSenderRegistrar>());

        serviceCollection.AddTransient<IMessageService, MessageService>();
        serviceCollection.AddTransient<IMessageCrudService, MessageCrudService>();
        serviceCollection.AddTransient<IMessageSearchService, MessageSearchService>();

        serviceCollection.AddTransient<ICommunicationUserService, CommunicationUserService>();
        serviceCollection.AddTransient<ICommunicationUserCrudService, CommunicationUserCrudService>();
        serviceCollection.AddTransient<ICommunicationUserSearchService, CommunicationUserSearchService>();

        serviceCollection.AddTransient<IConversationService, ConversationService>();
        serviceCollection.AddTransient<IConversationCrudService, ConversationCrudService>();
        serviceCollection.AddTransient<IConversationSearchService, ConversationSearchService>();

        serviceCollection.AddTransient<MemberChangedEventHandler>();

        serviceCollection.AddTransient<PushNotificationSender>();

        serviceCollection.AddTransient<CommunicationExportImport>();

        serviceCollection.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Anchor>());
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        _appBuilder = appBuilder;

        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        appBuilder.RegisterEventHandler<MemberChangedEvent, MemberChangedEventHandler>();

        // Register permissions
        //var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
        //permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "CommunicationModule", ModuleConstants.Security.Permissions.AllPermissions);

        // Message senders
        var messageSendersRegistrar = appBuilder.ApplicationServices.GetService<IMessageSenderRegistrar>();
        messageSendersRegistrar.Register<PushNotificationSender>(() => appBuilder.ApplicationServices
            .GetService<PushNotificationSender>());

        ModuleConstants.Settings.General.MessageSenders.AllowedValues = messageSendersRegistrar.AllRegisteredSenders.Select(x => x.SenderName).ToArray();

        // Apply migrations
        using var serviceScope = serviceProvider.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<CommunicationDbContext>();
        dbContext.Database.Migrate();

    }

    public void Uninstall()
    {
        // Nothing to do here
    }

    public Task ExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback,
        ICancellationToken cancellationToken)
    {
        return _appBuilder.ApplicationServices.GetRequiredService<CommunicationExportImport>().DoExportAsync(outStream, options,
            progressCallback, cancellationToken);
    }

    public Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback,
        ICancellationToken cancellationToken)
    {
        return _appBuilder.ApplicationServices.GetRequiredService<CommunicationExportImport>().DoImportAsync(inputStream, options,
            progressCallback, cancellationToken);
    }

}
