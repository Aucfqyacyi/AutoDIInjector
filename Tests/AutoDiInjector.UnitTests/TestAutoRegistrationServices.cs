using AutoDIInjector;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestedAssemly;

namespace AutoDiInjector.UnitTests;



public class TestAutoRegistrationServices
{
    
    [Fact]
    public void AddServices_OneAssembly_CorrectCountServices()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.Equal(6, services.Count());
        Assert.Equal(2, services.Count(service=> service.Lifetime == ServiceLifetime.Singleton));
        Assert.Equal(2, services.Count(service => service.Lifetime == ServiceLifetime.Scoped));
        Assert.Equal(2, services.Count(service => service.Lifetime == ServiceLifetime.Transient));
    }

    [Fact]
    public void AddServices_OneAssemblyWithPredicate_CorrectCountServices()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(type => type.Name.Contains('S') || type.Name.Contains('2'), Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.Equal(5, services.Count());
        Assert.Equal(2, services.Count(service => service.Lifetime == ServiceLifetime.Singleton));
        Assert.Equal(2, services.Count(service => service.Lifetime == ServiceLifetime.Scoped));
        Assert.Equal(1, services.Count(service => service.Lifetime == ServiceLifetime.Transient));
    }

    [Fact]
    public void AddServices_TwoAssemblies_CorrectCountServices()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)), Assembly.GetAssembly(typeof(AnotherTestedAssemly.ScopedClass1)));

        //Assert
        Assert.Equal(12, services.Count());
        Assert.Equal(4, services.Count(service => service.Lifetime == ServiceLifetime.Singleton));
        Assert.Equal(4, services.Count(service => service.Lifetime == ServiceLifetime.Scoped));
        Assert.Equal(4, services.Count(service => service.Lifetime == ServiceLifetime.Transient));
    }
    
    [Fact]
    public void AddServices_OneAssembly_CorrectServiceLifetimes()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(IScopedClass) && 
                                                          service.Lifetime == ServiceLifetime.Scoped));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(ITransientClass) && 
                                                          service.Lifetime == ServiceLifetime.Transient));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(ISingletonClass) && 
                                                          service.Lifetime == ServiceLifetime.Singleton));

        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(ITransientClass) &&
                                                       service.Lifetime == ServiceLifetime.Scoped));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(ISingletonClass) &&
                                                       service.Lifetime == ServiceLifetime.Transient));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(IScopedClass) &&
                                                       service.Lifetime == ServiceLifetime.Singleton));
    }

    [Fact]
    public void AddServices_OneAssembly_CorrectServiceImplementationTypes()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(IScopedClass) &&
                                                          service.ImplementationType == typeof(ScopedClass1)));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(ITransientClass) &&
                                                          service.ImplementationType == typeof(TransientClass1)));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(ISingletonClass) &&
                                                          service.ImplementationType == typeof(SingletonClass1)));

        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(IScopedClass) &&
                                                       service.ImplementationType == typeof(ScopedClass2)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(ITransientClass) &&
                                                       service.ImplementationType == typeof(TransientClass2)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(ISingletonClass) &&
                                                       service.ImplementationType == typeof(SingletonClass2)));
    }

    [Fact]
    public void AddServices_OneAssembly_NotRegisterIncorrectServices()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert

        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(DefaultClass)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(DefaultGenericClass<int>)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(DefaultAbstractGenericClass<int>)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(DefaultAbstractClass)));
    }


}