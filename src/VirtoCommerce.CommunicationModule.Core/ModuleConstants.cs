using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VirtoCommerce.CommunicationModule.Core.MessageSenders;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CommunicationModule.Core;
[ExcludeFromCodeCoverage]
public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Access = "CommunicationModule:access";
            public const string Create = "CommunicationModule:create";
            public const string Read = "CommunicationModule:read";
            public const string Update = "CommunicationModule:update";
            public const string Delete = "CommunicationModule:delete";

            public static string[] AllPermissions { get; } =
            {
                Access,
                Create,
                Read,
                Update,
                Delete,
            };
        }
    }

    public static class CommunicationUserType
    {
        public const string Organization = "Organization";
        public const string Employee = "Employee";
        public const string Customer = "Customer";
    }

    public static class EntityType
    {
        public const string Product = "Product";
        public const string Order = "Order";
    }

    public static class ReadStatus
    {
        public const string New = "New";
        public const string Read = "Read";
        public const string NotRead = "NotRead";
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor CommunicationUserTypes { get; } = new()
            {
                Name = "Communication.UserTypes",
                GroupName = "Communication|General",
                ValueType = SettingValueType.ShortText,
                IsDictionary = true,
                IsLocalizable = true,
                AllowedValues =
                    [
                        CommunicationUserType.Organization,
                        CommunicationUserType.Employee,
                        CommunicationUserType.Customer,
                    ]
            };

            public static SettingDescriptor MessageSenders { get; } = new()
            {
                Name = "Communication.MessageSenders",
                GroupName = "Communication|General",
                ValueType = SettingValueType.ShortText,
                DefaultValue = nameof(PushNotificationSender)
                //IsDictionary = true,
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return CommunicationUserTypes;
                    yield return MessageSenders;
                }
            }
        }

        public static IEnumerable<SettingDescriptor> AllSettings
        {
            get
            {
                return General.AllGeneralSettings;
            }
        }
    }
}
