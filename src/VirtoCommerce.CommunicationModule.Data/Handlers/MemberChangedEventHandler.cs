using System.Threading.Tasks;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CustomerModule.Core.Events;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.CommunicationModule.Data.Handlers;
public class MemberChangedEventHandler : IEventHandler<MemberChangedEvent>
{
    private readonly ICommunicationUserService _communicationUserService;
    private readonly ICommunicationUserCrudService _communicationUserCrudService;

    public MemberChangedEventHandler(
        ICommunicationUserService communicationUserService,
        ICommunicationUserCrudService communicationUserCrudService
        )
    {
        _communicationUserService = communicationUserService;
        _communicationUserCrudService = communicationUserCrudService;
    }

    public virtual async Task Handle(MemberChangedEvent message)
    {
        var changedMembers = message.ChangedEntries;
        foreach (var member in changedMembers)
        {
            if (member.EntryState == Platform.Core.Common.EntryState.Modified)
            {
                var newData = member.NewEntry;
                var communicationUser = await _communicationUserService.GetCommunicationUserByUserId(newData.Id, Core.ModuleConstants.CommunicationUserType.Organization);
                if (communicationUser != null)
                {
                    communicationUser.UserName = newData.Name;
                    communicationUser.AvatarUrl = newData.IconUrl;

                    await _communicationUserCrudService.SaveChangesAsync([communicationUser]);
                }
            }
        }
    }
}
