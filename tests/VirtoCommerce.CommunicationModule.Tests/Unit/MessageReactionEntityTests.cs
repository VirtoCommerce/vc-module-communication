using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Unit;

[ExcludeFromCodeCoverage]
public class MessageReactionEntityTests
{
    [Fact]
    public void ConvertMessageReactionEntityToModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageReactionEntity = new MessageReactionEntity();

        // Act
        Action actual = () => messageReactionEntity.ToModel(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ConvertMessageReactionEntityFromModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageReactionEntity = new MessageReactionEntity();

        // Act
        Action actual = () => messageReactionEntity.FromModel(null, null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void ConvertMessageReactionEntityFromModelToModel_NotNull_ReturnsSameValue(MessageReactionEntity originalMessageReactionEntity)
    {
        // Arrange
        var pkMap = new Mock<PrimaryKeyResolvingMap>();

        // Act
        var convertedMessageReaction = originalMessageReactionEntity.ToModel(new MessageReaction());
        var convertedMessageReactionEntity = new MessageReactionEntity().FromModel(convertedMessageReaction, pkMap.Object);

        // Assertion
        Assert.Equal(originalMessageReactionEntity.Id, convertedMessageReactionEntity.Id);
        Assert.Equal(originalMessageReactionEntity.MessageId, convertedMessageReactionEntity.MessageId);
        Assert.Equal(originalMessageReactionEntity.UserId, convertedMessageReactionEntity.UserId);
        Assert.Equal(originalMessageReactionEntity.Reaction, convertedMessageReactionEntity.Reaction);
        Assert.Equal(originalMessageReactionEntity.CreatedDate, convertedMessageReactionEntity.CreatedDate);
        Assert.Equal(originalMessageReactionEntity.ModifiedDate, convertedMessageReactionEntity.ModifiedDate);
        Assert.Equal(originalMessageReactionEntity.CreatedBy, convertedMessageReactionEntity.CreatedBy);
        Assert.Equal(originalMessageReactionEntity.ModifiedBy, convertedMessageReactionEntity.ModifiedBy);
    }

    [Fact]
    public void PatchMessageReactionEntity_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageReactionEntity = new MessageReactionEntity();

        // Act
        Action actual = () => messageReactionEntity.Patch(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void PatchMessageReactionEntity_NotNull_ReturnsActualValue(MessageReactionEntity actualMessageReactionEntity)
    {
        // Arrange
        var patchedMessageAttachmentEntity = new MessageReactionEntity();

        // Act
        actualMessageReactionEntity.Patch(patchedMessageAttachmentEntity);

        // Assertion
        Assert.Equal(actualMessageReactionEntity.MessageId, patchedMessageAttachmentEntity.MessageId);
        Assert.Equal(actualMessageReactionEntity.UserId, patchedMessageAttachmentEntity.UserId);
        Assert.Equal(actualMessageReactionEntity.Reaction, patchedMessageAttachmentEntity.Reaction);
    }

    public static TheoryData<MessageReactionEntity> Input()
    {
        return new TheoryData<MessageReactionEntity>()
        {
            new MessageReactionEntity
            {
                Id = "TestMessageAttachmentId",
                MessageId = "TestMessageId",
                UserId = "TestUserId",
                Reaction = "TestReaction",
                CreatedDate = new DateTime(2024, 11, 01),
                ModifiedDate = new DateTime(2024, 11, 01),
                CreatedBy = "Test Created By",
                ModifiedBy = "Test Modified By"
            }
        };
    }

}
