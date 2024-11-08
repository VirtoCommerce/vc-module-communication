using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Unit;

[ExcludeFromCodeCoverage]
public class MessageRecipientEntityTests
{
    [Fact]
    public void ConvertMessageRecipientEntityToModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        MessageRecipientEntity messageRecipientEntity = new MessageRecipientEntity();

        // Act
        Action actual = () => messageRecipientEntity.ToModel(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ConvertMessageRecipientEntityFromModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        MessageRecipientEntity messageRecipientEntity = new MessageRecipientEntity();

        // Act
        Action actual = () => messageRecipientEntity.FromModel(null, null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void ConvertMessageRecipientEntityFromModelToModel_NotNull_ReturnsSameValue(MessageRecipientEntity originalMessageRecipientEntity)
    {
        // Arrange
        var pkMap = new Mock<PrimaryKeyResolvingMap>();

        // Act
        var convertedMessageRecipient = originalMessageRecipientEntity.ToModel(new MessageRecipient());
        var convertedMessageRecipientEntity = new MessageRecipientEntity().FromModel(convertedMessageRecipient, pkMap.Object);

        // Assertion
        Assert.Equal(originalMessageRecipientEntity.Id, convertedMessageRecipientEntity.Id);
        Assert.Equal(originalMessageRecipientEntity.MessageId, convertedMessageRecipientEntity.MessageId);
        Assert.Equal(originalMessageRecipientEntity.RecipientId, convertedMessageRecipientEntity.RecipientId);
        Assert.Equal(originalMessageRecipientEntity.ReadStatus, convertedMessageRecipientEntity.ReadStatus);
        Assert.Equal(originalMessageRecipientEntity.ReadTimestamp, convertedMessageRecipientEntity.ReadTimestamp);
        Assert.Equal(originalMessageRecipientEntity.CreatedDate, convertedMessageRecipientEntity.CreatedDate);
        Assert.Equal(originalMessageRecipientEntity.ModifiedDate, convertedMessageRecipientEntity.ModifiedDate);
        Assert.Equal(originalMessageRecipientEntity.CreatedBy, convertedMessageRecipientEntity.CreatedBy);
        Assert.Equal(originalMessageRecipientEntity.ModifiedBy, convertedMessageRecipientEntity.ModifiedBy);
    }

    [Fact]
    public void PatchMessageRecipientEntity_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageRecipientEntity = new MessageRecipientEntity();

        // Act
        Action actual = () => messageRecipientEntity.Patch(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void PatchMessageRecipientEntity_NotNull_ReturnsActualValue(MessageRecipientEntity actualMessageRecipientEntity)
    {
        // Arrange
        var patchedMessageRecipientEntity = new MessageRecipientEntity();

        // Act
        actualMessageRecipientEntity.Patch(patchedMessageRecipientEntity);

        // Assertion
        Assert.Equal(actualMessageRecipientEntity.MessageId, patchedMessageRecipientEntity.MessageId);
        Assert.Equal(actualMessageRecipientEntity.RecipientId, patchedMessageRecipientEntity.RecipientId);
        Assert.Equal(actualMessageRecipientEntity.ReadStatus, patchedMessageRecipientEntity.ReadStatus);
        Assert.Equal(actualMessageRecipientEntity.ReadTimestamp, patchedMessageRecipientEntity.ReadTimestamp);
    }

    public static TheoryData<MessageRecipientEntity> Input()
    {
        return new TheoryData<MessageRecipientEntity>()
        {
            new MessageRecipientEntity
            {
                Id = "TestMessageAttachmentId",
                MessageId = "TestMessageId",
                RecipientId = "TestRecipientId",
                ReadStatus = "TestReadStatus",
                ReadTimestamp = new DateTime(2024, 10, 31),
                CreatedDate = new DateTime(2024, 10, 31),
                ModifiedDate = new DateTime(2024, 10, 31),
                CreatedBy = "Test Created By",
                ModifiedBy = "Test Modified By"
            }
        };
    }

}
