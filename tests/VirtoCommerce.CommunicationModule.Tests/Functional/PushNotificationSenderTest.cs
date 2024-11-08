using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CommunicationModule.Core.MessageSenders;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;
using VirtoCommerce.Platform.Core.Security.Search;
using Xunit;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class PushNotificationSenderTest
{
    [Fact]
    public async Task SendMessageByPushNotificationSender()
    {
        // Arrange
        var serviceProvider = new ServiceBuilder().GetServiceCollection().BuildServiceProvider();

        var communicationUserCrudServiceMock = serviceProvider.GetService<ICommunicationUserCrudService>();
        var pushNotificationManagerMock = new PushNotificationManagerMock();

        var pushNotificationSender = new PushNotificationSender(
            communicationUserCrudServiceMock,
            serviceProvider.GetService<IUserSearchService>(),
            pushNotificationManagerMock
            );

        var message = new Message
        {
            Id = "TestMessageId",
            SenderId = "TestSenderId",
            EntityId = "TestEntityId",
            EntityType = "TestEntityType",
            Content = "My test message content",
            ThreadId = "TestThreadId",
            Recipients = new List<MessageRecipient>
            {
                new MessageRecipient
                {
                    RecipientId = "TestUserId"
                },
                new MessageRecipient
                {
                    RecipientId = "TestOrganizationId"
                }
            }
        };

        var communicationUserEmployee = new CommunicationUser
        {
            Id = "TestUserId",
            UserType = "Employee"
        };
        var communicationUserOrganization = new CommunicationUser
        {
            Id = "TestOrganizationId",
            UserType = "Organization"
        };
        await communicationUserCrudServiceMock.SaveChangesAsync([communicationUserEmployee, communicationUserOrganization]);

        // Act
        var sendResult = await pushNotificationSender.SendMessage(message);

        // Assertion
        sendResult.Should().NotBeNull();
        sendResult.Status.Should().Be("Success");
        pushNotificationManagerMock.Notifications.Count.Should().Be(2);
    }
}
