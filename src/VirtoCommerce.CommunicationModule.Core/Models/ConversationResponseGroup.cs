using System;

namespace VirtoCommerce.CommunicationModule.Core.Models;
[Flags]
public enum ConversationResponseGroup
{
    None = 0,
    WithLastMessage = 1,
    WithUnreadCount = 2,
    Full = None | WithLastMessage | WithUnreadCount
}
