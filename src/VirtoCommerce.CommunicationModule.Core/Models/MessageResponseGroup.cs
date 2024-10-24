using System;

namespace VirtoCommerce.CommunicationModule.Core.Models;
[Flags]
public enum MessageResponseGroup
{
    None = 0,
    WithAttachments = 1,
    WithRecipients = 2,
    WithReactions = 4,
    WithAnswers = 8,
    WithoutAnswers = None | WithAttachments | WithRecipients | WithReactions,
    Full = None | WithAttachments | WithRecipients | WithReactions | WithAnswers
}
