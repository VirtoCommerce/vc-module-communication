using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Unit;

[ExcludeFromCodeCoverage]
public class MessageEntityTests
{
    [Fact]
    public void ConvertMessageEntityToModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageEntity = new MessageEntity();

        // Act
        Action actual = () => messageEntity.ToModel(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ConvertMessageEntityFromModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageEntity = new MessageEntity();

        // Act
        Action actual = () => messageEntity.FromModel(null, null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void ConvertMessageEntityFromModelToModel_NotNull_ReturnsSameValue(MessageEntity originalMessageEntity)
    {
        // Arrange
        var pkMap = new Mock<PrimaryKeyResolvingMap>();

        // Act
        var convertedMessage = originalMessageEntity.ToModel(new Message());
        var convertedMessageEntity = new MessageEntity().FromModel(convertedMessage, pkMap.Object);

        // Assertion
        Assert.Equal(originalMessageEntity.Id, convertedMessageEntity.Id);
        Assert.Equal(originalMessageEntity.SenderId, convertedMessageEntity.SenderId);
        Assert.Equal(originalMessageEntity.ConversationId, convertedMessageEntity.ConversationId);
        Assert.Equal(originalMessageEntity.Content, convertedMessageEntity.Content);
        Assert.Equal(originalMessageEntity.ThreadId, convertedMessageEntity.ThreadId);
        Assert.Equal(originalMessageEntity.CreatedDate, convertedMessageEntity.CreatedDate);
        Assert.Equal(originalMessageEntity.ModifiedDate, convertedMessageEntity.ModifiedDate);
        Assert.Equal(originalMessageEntity.CreatedBy, convertedMessageEntity.CreatedBy);
        Assert.Equal(originalMessageEntity.ModifiedBy, convertedMessageEntity.ModifiedBy);
        Assert.Equal(originalMessageEntity.Attachments, convertedMessageEntity.Attachments);
        Assert.Equal(originalMessageEntity.Recipients, convertedMessageEntity.Recipients);
        Assert.Equal(originalMessageEntity.Reactions, convertedMessageEntity.Reactions);
    }

    [Fact]
    public void PatchMessageEntity_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageEntity = new MessageEntity();

        // Act
        Action actual = () => messageEntity.Patch(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void PatchMessageEntity_NotNull_ReturnsActualValue(MessageEntity actualMessageEntity)
    {
        // Arrange
        var patchedMessageEntity = new MessageEntity();

        // Act
        actualMessageEntity.Patch(patchedMessageEntity);

        // Assertion
        Assert.Equal(actualMessageEntity.SenderId, patchedMessageEntity.SenderId);
        Assert.Equal(actualMessageEntity.ConversationId, patchedMessageEntity.ConversationId);
        Assert.Equal(actualMessageEntity.Content, patchedMessageEntity.Content);
        Assert.Equal(actualMessageEntity.ThreadId, patchedMessageEntity.ThreadId);
        Assert.Equal(actualMessageEntity.Attachments, patchedMessageEntity.Attachments);
        Assert.Equal(actualMessageEntity.Recipients, patchedMessageEntity.Recipients);
        Assert.Equal(actualMessageEntity.Reactions, patchedMessageEntity.Reactions);
    }

    public static TheoryData<MessageEntity> Input()
    {
        return new TheoryData<MessageEntity>()
        {
            new MessageEntity
            {
                Id = "TestMessageId",
                SenderId = "TestSenderId",
                ConversationId = "TestConversationId",
                Content = "My test message content",
                ThreadId = "TestThreadId",
                CreatedDate = new DateTime(2024, 11, 01),
                ModifiedDate = new DateTime(2024, 11, 01),
                CreatedBy = "Test Created By",
                ModifiedBy = "Test Modified By",
                Conversation = new ConversationEntity{
                    Id = "TestConversationId",
                    EntityId = "TestEntityId",
                },
                Attachments = new ObservableCollection<MessageAttachmentEntity>(
                    new List<MessageAttachmentEntity> {
                        new MessageAttachmentEntity
                        {
                            Id = "TestMessageAttachmentId",
                            MessageId = "TestMessageId",
                            AttachmentUrl = "attachment-url.test",
                            FileType = "TestFileType",
                            FileSize = 42,
                            CreatedDate = new DateTime(2024, 11, 01),
                            ModifiedDate = new DateTime(2024, 11, 01),
                            CreatedBy = "Test Created By",
                            ModifiedBy = "Test Modified By"
                        }
                    }),
                Recipients = new ObservableCollection<MessageRecipientEntity>(
                    new List<MessageRecipientEntity> {
                        new MessageRecipientEntity
                        {
                            Id = "TestMessageRecipientId",
                            MessageId = "TestMessageId",
                            RecipientId = "TestRecipientId",
                            ReadStatus = "TestReadStatus",
                            ReadTimestamp = new DateTime(2024, 11, 01),
                            CreatedDate = new DateTime(2024, 11, 01),
                            ModifiedDate = new DateTime(2024, 11, 01),
                            CreatedBy = "Test Created By",
                            ModifiedBy = "Test Modified By"
                        }
                    }),
                Reactions = new ObservableCollection<MessageReactionEntity>(
                    new List<MessageReactionEntity> {
                        new MessageReactionEntity
                        {
                            Id = "TestMessageReactionId",
                            MessageId = "TestMessageId",
                            UserId = "TestUserId",
                            Reaction = "TestReaction",
                            CreatedDate = new DateTime(2024, 11, 01),
                            ModifiedDate = new DateTime(2024, 11, 01),
                            CreatedBy = "Test Created By",
                            ModifiedBy = "Test Modified By"
                        }
                    })
            }
        };
    }

}
