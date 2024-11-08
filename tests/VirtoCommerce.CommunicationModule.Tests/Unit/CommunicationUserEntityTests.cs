using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Unit;

[ExcludeFromCodeCoverage]
public class CommunicationUserEntityTests
{
    [Fact]
    public void ConvertCommunicationUserEntityToModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        CommunicationUserEntity communicationUserEntity = new CommunicationUserEntity();

        // Act
        Action actual = () => communicationUserEntity.ToModel(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ConvertCommunicationUserEntityFromModel_Null_ThrowsArgumentNullException()
    {
        // Arrange
        CommunicationUserEntity communicationUserEntity = new CommunicationUserEntity();

        // Act
        Action actual = () => communicationUserEntity.FromModel(null, null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void ConvertCommunicationUserEntityFromModelToModel_NotNull_ReturnsSameValue(CommunicationUserEntity originalCommunicationUserEntity)
    {
        // Arrange
        var pkMap = new Mock<PrimaryKeyResolvingMap>();

        // Act
        var convertedCommunicationUser = originalCommunicationUserEntity.ToModel(new CommunicationUser());
        var convertedCommunicationUserEntity = new CommunicationUserEntity().FromModel(convertedCommunicationUser, pkMap.Object);

        // Assertion
        Assert.Equal(originalCommunicationUserEntity.Id, convertedCommunicationUserEntity.Id);
        Assert.Equal(originalCommunicationUserEntity.UserName, convertedCommunicationUserEntity.UserName);
        Assert.Equal(originalCommunicationUserEntity.UserId, convertedCommunicationUserEntity.UserId);
        Assert.Equal(originalCommunicationUserEntity.UserType, convertedCommunicationUserEntity.UserType);
        Assert.Equal(originalCommunicationUserEntity.AvatarUrl, convertedCommunicationUserEntity.AvatarUrl);
        Assert.Equal(originalCommunicationUserEntity.CreatedDate, convertedCommunicationUserEntity.CreatedDate);
        Assert.Equal(originalCommunicationUserEntity.ModifiedDate, convertedCommunicationUserEntity.ModifiedDate);
        Assert.Equal(originalCommunicationUserEntity.CreatedBy, convertedCommunicationUserEntity.CreatedBy);
        Assert.Equal(originalCommunicationUserEntity.ModifiedBy, convertedCommunicationUserEntity.ModifiedBy);
    }

    [Fact]
    public void PatchCommunicationUserEntity_Null_ThrowsArgumentNullException()
    {
        // Arrange
        var communicationUserEntity = new CommunicationUserEntity();

        // Act
        Action actual = () => communicationUserEntity.Patch(null);

        // Assertion
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Theory]
    [MemberData(nameof(Input))]
    public void PatchCommunicationUserEntity_NotNull_ReturnsActualValue(CommunicationUserEntity actualCommunicationUserEntity)
    {
        // Arrange
        var patchedCommunicationUserEntity = new CommunicationUserEntity();

        // Act
        actualCommunicationUserEntity.Patch(patchedCommunicationUserEntity);

        // Assertion
        Assert.Equal(actualCommunicationUserEntity.UserName, patchedCommunicationUserEntity.UserName);
        Assert.Equal(actualCommunicationUserEntity.UserId, patchedCommunicationUserEntity.UserId);
        Assert.Equal(actualCommunicationUserEntity.UserType, patchedCommunicationUserEntity.UserType);
        Assert.Equal(actualCommunicationUserEntity.AvatarUrl, patchedCommunicationUserEntity.AvatarUrl);
    }

    public static TheoryData<CommunicationUserEntity> Input()
    {
        return new TheoryData<CommunicationUserEntity>()
        {
            new CommunicationUserEntity
            {
                Id = "TestCommunicationUserId",
                UserName = "My test communication user",
                UserId = "TestUserId",
                UserType = "TestUserType",
                AvatarUrl = "avatat-url.test",
                CreatedDate = new DateTime(2024, 10, 31),
                ModifiedDate = new DateTime(2024, 10, 31),
                CreatedBy = "Test Created By",
                ModifiedBy = "Test Modified By"
            }
        };
    }

}
