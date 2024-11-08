using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.CommunicationModule.Data.Handlers;
using VirtoCommerce.CustomerModule.Core.Events;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class MemberChangedEventHandlerTest
{
    [Fact]
    public async Task HandleMemberChanged()
    {
        // Arrange
        var serviceProvider = new ServiceBuilder().GetServiceCollection().BuildServiceProvider();
        var communicationUserCrudServiceMock = new CommunicationUserCrudServiceMock();

        var handler = new MemberChangedEventHandler(
            serviceProvider.GetService<ICommunicationUserService>(),
            communicationUserCrudServiceMock
            );

        var memberChangedEvent = new MemberChangedEvent
        (
            new List<GenericChangedEntry<Member>>
            {
                new GenericChangedEntry<Member>
                (
                    new Employee
                    {
                        Id = "TestUserId",
                        Name = "NewTestUserName",
                        IconUrl = "new-icon.url"
                    },
                    null,
                    EntryState.Modified
                )
            }
        );

        // Act
        await handler.Handle(memberChangedEvent);

        // Assertion
        communicationUserCrudServiceMock.Models.FirstOrDefault(x => x.UserId == "TestUserId").Should().NotBeNull();
        communicationUserCrudServiceMock.Models.FirstOrDefault(x => x.UserId == "TestUserId").UserName.Should().Be("NewTestUserName");
        communicationUserCrudServiceMock.Models.FirstOrDefault(x => x.UserId == "TestUserId").AvatarUrl.Should().Be("new-icon.url");

    }
}
