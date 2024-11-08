using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;
using static VirtoCommerce.CommunicationModule.Core.ModuleConstants;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class SettingsManagerMock : ISettingsManager
{
    public IEnumerable<SettingDescriptor> AllRegisteredSettings => throw new System.NotImplementedException();

    public IEnumerable<ObjectSettingEntry> AllSettings => new List<ObjectSettingEntry>
        {
            new(Settings.General.MessageSenders) { Value = nameof(TestMessageSender) }
        };

    public Task<T> GetValueAsync<T>(SettingDescriptor settingDescriptor)
    {
        var setting = AllSettings.FirstOrDefault(x => x.Name == settingDescriptor.Name);
        return Task.FromResult((T)setting.Value);
    }

    public Task<ObjectSettingEntry> GetObjectSettingAsync(string name, string objectType = null, string objectId = null)
    {
        var setting = AllSettings.FirstOrDefault(x => x.Name == name);
        return Task.FromResult(setting);
    }

    public Task<IEnumerable<ObjectSettingEntry>> GetObjectSettingsAsync(IEnumerable<string> names, string objectType = null, string objectId = null)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<SettingDescriptor> GetSettingsForType(string typeName)
    {
        throw new System.NotImplementedException();
    }

    public void RegisterSettings(IEnumerable<SettingDescriptor> settings, string moduleId = null)
    {
        throw new System.NotImplementedException();
    }

    public void RegisterSettingsForType(IEnumerable<SettingDescriptor> settings, string typeName)
    {
        throw new System.NotImplementedException();
    }

    public Task RemoveObjectSettingsAsync(IEnumerable<ObjectSettingEntry> objectSettings)
    {
        throw new System.NotImplementedException();
    }

    public Task SaveObjectSettingsAsync(IEnumerable<ObjectSettingEntry> objectSettings)
    {
        throw new System.NotImplementedException();
    }
}
