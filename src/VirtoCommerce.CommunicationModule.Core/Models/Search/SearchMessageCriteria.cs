using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CommunicationModule.Core.Models.Search;
public class SearchMessageCriteria : SearchCriteriaBase
{
    public string EntityId { get; set; }

    public string EntityType { get; set; }
}
