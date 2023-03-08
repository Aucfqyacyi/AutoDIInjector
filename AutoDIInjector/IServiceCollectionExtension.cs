using AutoDIInjector.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutoDIInjector;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services.AddServices(null, assemblies);
    }

    public static IServiceCollection AddServices(this IServiceCollection services, Func<Type, bool>? predicate, params Assembly[] assemblies)
    {
        Assembly current = Assembly.GetCallingAssembly();
        if (assemblies.Length == 0)
            return services.RegisterTypes(current.GetExportedTypes(), predicate);
        List<Assembly> all = new(assemblies) { current };
        return services.RegisterTypes(all.SelectMany(assem => assem.GetExportedTypes()), predicate);
    }
    
    private static IServiceCollection RegisterTypes(this IServiceCollection services, IEnumerable<Type> types, Func<Type, bool>? predicate = null)
    {
        Type serviceAttributeType = typeof(ServiceAttribute);
        foreach (var type in types)
        {
            if (type.IsTypeToRegister() && (predicate?.Invoke(type) ?? true))
            {
                var attribute = type.GetCustomAttribute(serviceAttributeType) as ServiceAttribute;
                Type interfaceType = attribute!.InterfaceType ?? type;
                ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interfaceType, type, attribute!.ServiceLifetime);
                services.Add(serviceDescriptor);
            }
        }
        return services;
    }
}
