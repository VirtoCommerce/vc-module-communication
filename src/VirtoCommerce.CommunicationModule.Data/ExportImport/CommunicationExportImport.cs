using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Models.Search;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.CommunicationModule.Data.ExportImport;
public class CommunicationExportImport
{
    private readonly ICommunicationUserSearchService _communicationUserSearchService;
    private readonly ICommunicationUserCrudService _communicationUserCrudService;
    private readonly IConversationSearchService _conversationSearchService;
    private readonly IConversationCrudService _conversationCrudService;
    private readonly IMessageSearchService _messageSearchService;
    private readonly IMessageCrudService _messageCrudService;
    private readonly JsonSerializer _jsonSerializer;
    private readonly int _batchSize = 50;

    public CommunicationExportImport(
        ICommunicationUserSearchService communicationUserSearchService,
        ICommunicationUserCrudService communicationUserCrudService,
        IConversationSearchService conversationSearchService,
        IConversationCrudService conversationCrudService,
        IMessageSearchService messageSearchService,
        IMessageCrudService messageCrudService,
        JsonSerializer jsonSerializer
        )
    {
        _communicationUserSearchService = communicationUserSearchService;
        _communicationUserCrudService = communicationUserCrudService;
        _conversationSearchService = conversationSearchService;
        _conversationCrudService = conversationCrudService;
        _messageSearchService = messageSearchService;
        _messageCrudService = messageCrudService;
        _jsonSerializer = jsonSerializer;
    }

    public virtual async Task DoExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var progressInfo = new ExportImportProgressInfo { Description = "loading data..." };
        progressCallback(progressInfo);

        using (var sw = new StreamWriter(outStream))
        using (var writer = new JsonTextWriter(sw))
        {
            await writer.WriteStartObjectAsync();

            #region Export CommunicationUsers

            progressInfo.Description = "CommunicationUsers exporting...";
            progressCallback(progressInfo);

            await writer.WritePropertyNameAsync("CommunicationUsers");
            await writer.SerializeArrayWithPagingAsync(_jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchResult = await _communicationUserSearchService.SearchAsync(new SearchCommunicationUserCriteria { Skip = skip, Take = take });
                return (GenericSearchResult<CommunicationUser>)searchResult;
            }
            , (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} communication users have been exported";
                progressCallback(progressInfo);
            }, cancellationToken);

            #endregion

            #region Export Conversations

            progressInfo.Description = "Conversations exporting...";
            progressCallback(progressInfo);

            await writer.WritePropertyNameAsync("Conversations");
            await writer.SerializeArrayWithPagingAsync(_jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchResult = await _conversationSearchService.SearchAsync(new SearchConversationCriteria { Skip = skip, Take = take });
                return (GenericSearchResult<Conversation>)searchResult;
            }
            , (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} conversations have been exported";
                progressCallback(progressInfo);
            }, cancellationToken);

            #endregion

            #region Export Messages

            progressInfo.Description = "Messages exporting...";
            progressCallback(progressInfo);

            await writer.WritePropertyNameAsync("Messages");
            await writer.SerializeArrayWithPagingAsync(_jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchResult = await _messageSearchService.SearchAsync(new SearchMessageCriteria { Skip = skip, Take = take });
                return (GenericSearchResult<Message>)searchResult;
            }
            , (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} messages have been exported";
                progressCallback(progressInfo);
            }, cancellationToken);

            #endregion

            await writer.WriteEndObjectAsync();
            await writer.FlushAsync();
        }
    }

    public virtual async Task DoImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var progressInfo = new ExportImportProgressInfo();

        using (var streamReader = new StreamReader(inputStream))
        using (var reader = new JsonTextReader(streamReader))
        {
            while (await reader.ReadAsync())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                switch (reader.Value?.ToString())
                {
                    case "CommunicationUsers":
                        await reader.DeserializeArrayWithPagingAsync<CommunicationUser>(_jsonSerializer, _batchSize, items => _communicationUserCrudService.SaveChangesAsync(items), processedCount =>
                        {
                            progressInfo.Description = $"{processedCount} communication users have been imported";
                            progressCallback(progressInfo);
                        }, cancellationToken);
                        break;
                    case "Conversations":
                        await reader.DeserializeArrayWithPagingAsync<Conversation>(_jsonSerializer, _batchSize, items => _conversationCrudService.SaveChangesAsync(items), processedCount =>
                        {
                            progressInfo.Description = $"{processedCount} conversations have been imported";
                            progressCallback(progressInfo);
                        }, cancellationToken);
                        break;
                    case "Messages":
                        await reader.DeserializeArrayWithPagingAsync<Message>(_jsonSerializer, _batchSize, items => _messageCrudService.SaveChangesAsync(items), processedCount =>
                        {
                            progressInfo.Description = $"{processedCount} messages have been imported";
                            progressCallback(progressInfo);
                        }, cancellationToken);
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
