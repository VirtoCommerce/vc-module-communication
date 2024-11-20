using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Models.Search;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.CommunicationModule.Core.Services;
public interface IConversationSearchService : ISearchService<SearchConversationCriteria, SearchConversationResult, Conversation>
{
}
