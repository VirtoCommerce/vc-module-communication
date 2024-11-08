using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.CommunicationModule.Data.Repositories;
using VirtoCommerce.CommunicationModule.Data.Services;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class CommunicationUserServiceTests
{
    private static readonly string _testCommunicationUserId = "TestCommunicationUserId";
    private static readonly string _testCommunicationUserUserId = "TestCommunicationUserUserId";
    private static readonly string _testCommunicationUserUserType = "TestCommunicationUserUserType";
    private static readonly CommunicationUser _communicationUser = new()
    {
        Id = _testCommunicationUserId,
        UserId = _testCommunicationUserUserId,
        UserType = _testCommunicationUserUserType,
    };

    [Fact]
    public async Task GetCommunicationUserByUserIdTest_ExistedUser_ReturnsUser()
    {
        // Arrange
        CommunicationRepositoryMock communicationRepositoryMock = new();
        var pkMap = new Mock<PrimaryKeyResolvingMap>();
        var communicationUserEntity = new CommunicationUserEntity().FromModel(_communicationUser, pkMap.Object);
        communicationRepositoryMock.Add(communicationUserEntity);
        var communicationUserService = GetCommunicationUserService(communicationRepositoryMock);

        // Act
        var actualUser = await communicationUserService.GetCommunicationUserByUserId(_testCommunicationUserUserId, _testCommunicationUserUserType);

        // Assertion
        actualUser.Should().NotBeNull();
        actualUser.Id.Should().Be(_communicationUser.Id);
    }

    [Fact]
    public async Task GetCommunicationUserByUserIdTest_NotExistedUser_ReturnsNull()
    {
        // Arrange
        var communicationUserService = GetCommunicationUserService();

        // Act
        var actualUser = await communicationUserService.GetCommunicationUserByUserId("UnknownTestUserId", "UnknownTestUserType");

        // Assertion
        actualUser.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(CreateCommunicationUserTestInput))]
    public async Task CreateCommunicationUserTest(string userId, string userType, string expectedUserName, string expectedUserAvatar)
    {
        // Arrange
        var communicationUserService = GetCommunicationUserService();

        // Act
        var actualUser = await communicationUserService.CreateCommunicationUser(userId, userType);

        // Assertion
        actualUser.Should().NotBeNull();
        actualUser.UserName.Should().Be(expectedUserName);
        actualUser.AvatarUrl.Should().Be(expectedUserAvatar);
    }

    [Theory]
    [MemberData(nameof(CreateCommunicationUserTestInput))]
    public async Task GetOrCreateCommunicationUserTest(string userId, string userType, string expectedUserName, string expectedUserAvatar)
    {
        // Arrange
        var communicationUserService = GetCommunicationUserService();

        // Act
        var actualUser = await communicationUserService.GetOrCreateCommunicationUser(userId, userType);

        // Assertion
        actualUser.Should().NotBeNull();
        actualUser.UserName.Should().Be(expectedUserName);
        actualUser.AvatarUrl.Should().Be(expectedUserAvatar);
    }

    private CommunicationUserService GetCommunicationUserService(
        ICommunicationRepository communicationRepositoryMock = null,
        ICommunicationUserCrudService communicationUserCrudServiceMock = null
        )
    {
        var serviceProvider = new ServiceBuilder().GetServiceCollection().BuildServiceProvider();

        if (communicationRepositoryMock == null)
        {
            communicationRepositoryMock = serviceProvider.GetService<ICommunicationRepository>();
        }

        if (communicationUserCrudServiceMock == null)
        {
            communicationUserCrudServiceMock = serviceProvider.GetService<ICommunicationUserCrudService>();
        }

        var communicationUserService = new CommunicationUserService
            (
                () => communicationRepositoryMock,
                communicationUserCrudServiceMock,
                serviceProvider.GetService<IMemberSearchService>()
            );

        return communicationUserService;
    }

    public static TheoryData<string, string, string, string> CreateCommunicationUserTestInput()
    {
        return new TheoryData<string, string, string, string>
        {
            {
                "TestOrganizationUserId",
                "Organization",
                "TestOrganizationName",
                "organization-icon.url"
            },
            {
                "TestEmployeeUserId",
                "Employee",
                "TestEmployeeName",
                "employee-icon.url"
            },
            {
                "TestContactUserId",
                "Customer",
                "TestContactName",
                "contact-icon.url"
            }
        };
    }
}
