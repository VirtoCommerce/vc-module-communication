using System.Diagnostics.CodeAnalysis;
using VirtoCommerce.CommunicationModule.Core.Models;
using VirtoCommerce.CommunicationModule.Core.Services;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class CommunicationUserCrudServiceMock : CrudServiceMock<CommunicationUser>, ICommunicationUserCrudService
{
}
