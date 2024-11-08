using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Unit;

[ExcludeFromCodeCoverage]
public class MessageAttachmentEntityTests
{
    [Fact]
    public void ConvertMessageAttachmentEntityToModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageAttachmentEntity = new MessageAttachmentEntity();

        // Act
        Action actual = () => messageAttachmentEntity.ToModel(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ConvertMessageAttachmentEntityFromModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageAttachmentEntity = new MessageAttachmentEntity();

        // Act
        Action actual = () => messageAttachmentEntity.FromModel(null, null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void ConvertMessageAttachmentEntityFromModelToModel_NotNull_ReturnsSameValue(MessageAttachmentEntity originalMessageAttachmentEntity)
    {
        // Arrange
        var pkMap = new Mock<PrimaryKeyResolvingMap>();

        // Act
        var convertedMessageAttachment = originalMessageAttachmentEntity.ToModel(new MessageAttachment());
        var convertedMessageAttachmentEntity = new MessageAttachmentEntity().FromModel(convertedMessageAttachment, pkMap.Object);

        // Assertion
        Assert.Equal(originalMessageAttachmentEntity.Id, convertedMessageAttachmentEntity.Id);
        Assert.Equal(originalMessageAttachmentEntity.MessageId, convertedMessageAttachmentEntity.MessageId);
        Assert.Equal(originalMessageAttachmentEntity.AttachmentUrl, convertedMessageAttachmentEntity.AttachmentUrl);
        Assert.Equal(originalMessageAttachmentEntity.FileType, convertedMessageAttachmentEntity.FileType);
        Assert.Equal(originalMessageAttachmentEntity.FileSize, convertedMessageAttachmentEntity.FileSize);
        Assert.Equal(originalMessageAttachmentEntity.CreatedDate, convertedMessageAttachmentEntity.CreatedDate);
        Assert.Equal(originalMessageAttachmentEntity.ModifiedDate, convertedMessageAttachmentEntity.ModifiedDate);
        Assert.Equal(originalMessageAttachmentEntity.CreatedBy, convertedMessageAttachmentEntity.CreatedBy);
        Assert.Equal(originalMessageAttachmentEntity.ModifiedBy, convertedMessageAttachmentEntity.ModifiedBy);
    }

    [Fact]
    public void PatchMessageAttachmentEntity_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var messageAttachmentEntity = new MessageAttachmentEntity();

        // Act
        Action actual = () => messageAttachmentEntity.Patch(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void PatchMessageAttachmentEntity_NotNull_ReturnsActualValue(MessageAttachmentEntity actualMessageAttachmentEntity)
    {
        // Arrange
        var patchedMessageAttachmentEntity = new MessageAttachmentEntity();

        // Act
        actualMessageAttachmentEntity.Patch(patchedMessageAttachmentEntity);

        // Assertion
        Assert.Equal(actualMessageAttachmentEntity.MessageId, patchedMessageAttachmentEntity.MessageId);
        Assert.Equal(actualMessageAttachmentEntity.AttachmentUrl, patchedMessageAttachmentEntity.AttachmentUrl);
        Assert.Equal(actualMessageAttachmentEntity.FileType, patchedMessageAttachmentEntity.FileType);
        Assert.Equal(actualMessageAttachmentEntity.FileSize, patchedMessageAttachmentEntity.FileSize);
    }

    public static TheoryData<MessageAttachmentEntity> Input()
    {
        return new TheoryData<MessageAttachmentEntity>()
        {
            new MessageAttachmentEntity
            {
                Id = "TestMessageAttachmentId",
                MessageId = "TestMessageId",
                AttachmentUrl = "attachment-url.test",
                FileType = "TestFileType",
                FileSize = 42,
                CreatedDate = new DateTime(2024, 10, 31),
                ModifiedDate = new DateTime(2024, 10, 31),
                CreatedBy = "Test Created By",
                ModifiedBy = "Test Modified By"
            }
        };
    }

}
