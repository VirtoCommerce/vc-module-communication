using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.CommunicationModule.Tests;

[ExcludeFromCodeCoverage]
public static class TestHepler
{
    public static IServiceCollection AddCollection<T>(this IServiceCollection services, T t) where T : class
    {
        return services.AddTransient(provider => t);
    }
}
