using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models.Search;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.CommunicationModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.GenericCrud;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class MessageSearchServiceTests
{
    [Theory]
    [MemberData(nameof(MessageSearchTestInput))]
    public async Task MessageSearchTest(string entityId, string entityType, string threadId, bool rootsOnly, int expectedMessagesCount)
    {
        // Arrange
        CommunicationRepositoryMock communicationRepositoryMock = new();
        foreach (var messageEntity in _messageEntities)
        {
            communicationRepositoryMock.Add(messageEntity);
        }

        var messageSearchService = GetMessageSearchService(communicationRepositoryMock);

        var criteria = new SearchMessageCriteria();
        criteria.EntityId = entityId;
        criteria.EntityType = entityType;
        criteria.ThreadId = threadId;
        criteria.RootsOnly = rootsOnly;
        criteria.Sort = "createdDate:asc";
        criteria.Skip = 0;
        criteria.Take = 20;

        // Act
        var actualSearchResult = await messageSearchService.SearchAsync(criteria);

        // Assertion
        actualSearchResult.Should().NotBeNull();
        actualSearchResult.TotalCount.Should().Be(expectedMessagesCount);
    }

    private MessageSearchService GetMessageSearchService(
        ICommunicationRepository communicationRepositoryMock = null
        )
    {
        var serviceProvider = new ServiceBuilder().GetServiceCollection().BuildServiceProvider();

        if (communicationRepositoryMock == null)
        {
            communicationRepositoryMock = serviceProvider.GetService<ICommunicationRepository>();
        }

        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);

        var messageSearchService = new MessageSearchService
            (
                () => communicationRepositoryMock,
                platformMemoryCache,
                serviceProvider.GetService<IMessageCrudService>(),
                serviceProvider.GetService<IOptions<CrudOptions>>()
            );

        return messageSearchService;
    }

    private static MessageEntity[] _messageEntities =
        [
            new MessageEntity
            {
                Id = "TestMessageId_1",
                EntityId = "TestEntityId_1",
                EntityType = "TestEntityType",
                ThreadId = null,
                Content = "Test message content 01"
            },
            new MessageEntity
            {
                Id = "TestMessageId_2",
                EntityId = "TestEntityId_2",
                EntityType = "TestEntityType",
                ThreadId = null,
                Content = "Test message content 02"
            },
            new MessageEntity
            {
                Id = "TestMessageId_3",
                EntityId = "TestEntityId_1",
                EntityType = "TestEntityType",
                ThreadId = "TestMessageId_1",
                Content = "Test message content 03"
            }
        ];

    public static TheoryData<string, string, string, bool, int> MessageSearchTestInput()
    {
        return new TheoryData<string, string, string, bool, int>
        {
            {
                "TestEntityId_2",
                null,
                null,
                false,
                1
            },
            {
                null,
                "TestEntityType",
                null,
                false,
                3
            },
            {
                null,
                null,
                "TestMessageId_1",
                false,
                1
            },
            {
                null,
                null,
                null,
                true,
                2
            }
        };
    }
}