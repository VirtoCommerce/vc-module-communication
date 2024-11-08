using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.CommunicationModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class MessageServiceTests
{
    private static readonly string _testMessageId = "TestMessageId";
    private static readonly string _testRecipientId = "TestRecipientId";
    private static readonly string _testUnreadRecipientId = "TestUnreadRecipientId";
    private static readonly string _testEntityId = "TestEntityId";
    private static readonly string _testEntityType = "TestEntityType";
    private static readonly Message _message = new()
    {
        Id = _testMessageId,
        SenderId = "TestSenderId",
        EntityId = _testEntityId,
        EntityType = _testEntityType,
        Content = "My test message content",
        ThreadId = "TestThreadId",
        Recipients = new List<MessageRecipient>
        {
            new MessageRecipient
            {
                Id = "TestMessageRecipientId",
                MessageId = _testMessageId,
                RecipientId = _testRecipientId,
                ReadStatus = "TestReadStatus",
                ReadTimestamp = new DateTime(2024, 11, 06),
            },
            new MessageRecipient
            {
                Id = "TestMessageRecipientId",
                MessageId = _testMessageId,
                RecipientId = _testUnreadRecipientId,
                ReadStatus = "New",
                ReadTimestamp = new DateTime(2024, 11, 07),
            }

        }
    };

    [Fact]
    public async Task SendNewMessageTest()
    {
        // Arrange
        MessageCrudServiceMock messageCrudServiceMock = new();
        var messageService = GetMessageService(null, messageCrudServiceMock);

        // Act
        await messageService.SendMessage(_message);

        // Assertion
        messageCrudServiceMock.Models.Where(x => x.Id == _testMessageId).Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateMessageTest()
    {
        // Arrange
        MessageCrudServiceMock messageCrudServiceMock = new();
        var messageService = GetMessageService(null, messageCrudServiceMock);
        var changedMessage = new Message
        {
            Id = _testMessageId,
            SenderId = "NewTestSenderId"
        };

        // Act
        await messageService.UpdateMessage(changedMessage);

        // Assertion
        messageCrudServiceMock.Models.FirstOrDefault(x => x.Id == _testMessageId).Should().NotBeNull();
        messageCrudServiceMock.Models.FirstOrDefault(x => x.Id == _testMessageId).SenderId.Should().Be("NewTestSenderId");
    }

    [Fact]
    public async Task DeleteMessageTestWithoutReplies()
    {
        // Arrange
        MessageCrudServiceMock messageCrudServiceMock = new();
        var messageService = GetMessageService(null, messageCrudServiceMock);

        // Act
        await messageService.DeleteMessage([_testMessageId], false);

        // Assertion
        messageCrudServiceMock.Models.FirstOrDefault(x => x.Id == _testMessageId).Should().BeNull();
    }

    [Fact]
    public async Task DeleteMessageTestWithReplies()
    {
        // Arrange
        MessageCrudServiceMock messageCrudServiceMock = new();
        var messageService = GetMessageService(null, messageCrudServiceMock);

        // Act
        await messageService.DeleteMessage([_testMessageId], true);

        // Assertion
        messageCrudServiceMock.Models.FirstOrDefault(x => x.Id == _testMessageId).Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(ReadTestInput))]
    public async Task SetMessageReadStatusTest(string messageId, string recipientId, string errorMessage)
    {
        // Arrange
        MessageCrudServiceMock messageCrudServiceMock = new();
        await messageCrudServiceMock.SaveChangesAsync([_message]);
        var messageService = GetMessageService(null, messageCrudServiceMock);
        Message message = null;

        // Act
        try
        {
            message = await messageService.SetMessageReadStatus(messageId, recipientId);
        }

        // Assertion
        catch (Exception ex)
        {
            ex.Message.Should().Contain(errorMessage);
            return;
        }
        message.Should().NotBeNull();
        message.Recipients.FirstOrDefault(x => x.RecipientId == _testRecipientId).ReadStatus.Should().Be("Read");
    }

    [Theory]
    [MemberData(nameof(ReactionTestInput))]
    public async Task SetMessageReactionTest(string messageId, string userId, string reaction, string errorMessage)
    {
        // Arrange
        MessageCrudServiceMock messageCrudServiceMock = new();
        await messageCrudServiceMock.SaveChangesAsync([_message]);
        var messageService = GetMessageService(null, messageCrudServiceMock);
        Message message = null;

        // Act
        try
        {
            message = await messageService.SetMessageReaction(messageId, userId, reaction);
        }

        // Assertion
        catch (Exception ex)
        {
            ex.Message.Should().Contain(errorMessage);
            return;
        }
        message.Should().NotBeNull();
        message.Reactions.FirstOrDefault(x => x.UserId == _testRecipientId).Reaction.Should().Be(reaction);
    }

    [Theory]
    [MemberData(nameof(UnreadCountTestInput))]
    public async Task UnreadCountTest(string recipientId, string entityId, string entityType, int expectedCount)
    {
        // Arrange
        CommunicationRepositoryMock communicationRepositoryMock = new();
        var pkMap = new Mock<PrimaryKeyResolvingMap>();
        var messageEntity = new MessageEntity().FromModel(_message, pkMap.Object);
        communicationRepositoryMock.Add(messageEntity);
        var messageService = GetMessageService(communicationRepositoryMock);

        // Act
        int actualCount = await messageService.GetUnreadMessagesCount(recipientId, entityId, entityType);

        // Assertion
        actualCount.Should().Be(expectedCount);
    }

    private MessageService GetMessageService(
        ICommunicationRepository communicationRepositoryMock = null,
        IMessageCrudService messageCrudServiceMock = null
        )
    {
        var serviceProvider = new ServiceBuilder().GetServiceCollection().BuildServiceProvider();

        var testMessageSender = new TestMessageSender();
        var messageSenderRegistrar = new MessageSenderRegistrarMock();
        messageSenderRegistrar.Register<TestMessageSender>(() => testMessageSender);

        if (communicationRepositoryMock == null)
        {
            communicationRepositoryMock = serviceProvider.GetService<ICommunicationRepository>();
        }

        if (messageCrudServiceMock == null)
        {
            messageCrudServiceMock = serviceProvider.GetService<IMessageCrudService>();
        }

        var messageService = new MessageService
            (
                () => communicationRepositoryMock,
                serviceProvider.GetService<ISettingsManager>(),
                messageSenderRegistrar,
                messageCrudServiceMock
            );

        return messageService;
    }

    public static TheoryData<string, string, string> ReadTestInput()
    {
        return new TheoryData<string, string, string>
        {
            {
                "UnknownMessageId",
                null,
                "Message with id UnknownMessageId not found"
            },
            {
                _testMessageId,
                "UnknownRecipientId",
                $"Recipient with id UnknownRecipientId not received message {_testMessageId}"
            },
            {
                _testMessageId,
                _testRecipientId,
                null
            }
        };
    }

    public static TheoryData<string, string, string, string> ReactionTestInput()
    {
        return new TheoryData<string, string, string, string>
        {
            {
                "UnknownMessageId",
                null,
                null,
                "Message with id UnknownMessageId not found"
            },
            {
                _testMessageId,
                _testRecipientId,
                "Like",
                null
            }
        };
    }

    public static TheoryData<string, string, string, int> UnreadCountTestInput()
    {
        return new TheoryData<string, string, string, int>
        {
            {
                _testUnreadRecipientId,
                _testEntityId,
                _testEntityType,
                1
            },
            {
                "UnknownRecipientId",
                "UnknownEntityId",
                "UnknownEntityType",
                0
            },
            {
                null,
                null,
                null,
                0
            }

        };
    }
}
