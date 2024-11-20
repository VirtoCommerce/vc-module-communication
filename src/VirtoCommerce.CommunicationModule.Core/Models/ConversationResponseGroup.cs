using System;

namespace VirtoCommerce.CommunicationModule.Core.Models;
[Flags]
public enum ConversationResponseGroup
{
    None = 0,
    WithLastMessage = 1
}
