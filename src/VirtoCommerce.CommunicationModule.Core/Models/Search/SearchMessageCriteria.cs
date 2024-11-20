using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models.Search;
public class SearchMessageCriteria : SearchCriteriaBase
{
    public string ConversationId { get; set; }

    public string ThreadId { get; set; }

    public bool RootsOnly { get; set; }
}
