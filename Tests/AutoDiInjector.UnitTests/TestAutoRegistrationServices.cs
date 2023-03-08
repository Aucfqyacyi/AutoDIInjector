using AutoDIInjector;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestedAssemly;

namespace AutoDiInjector.UnitTests;



public class TestAutoRegistrationServices
{
    [Fact]
    public void AddServices_OneAssemly_CorrectCountServices()
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
    public void AddServices_OneAssemlyWithPredicate_CorrectCountServices()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(type => type.Name.Contains('S') || type.Name.Contains('2'), Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.Equal(6, services.Count());
        Assert.Equal(2, services.Count(service => service.Lifetime == ServiceLifetime.Singleton));
        Assert.Equal(2, services.Count(service => service.Lifetime == ServiceLifetime.Scoped));
        Assert.Equal(1, services.Count(service => service.Lifetime == ServiceLifetime.Transient));
    }

    [Fact]
    public void AddServices_TwoAssemlies_CorrectCountServices()
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
    public void AddServices_OneAssemly_CorrectServiceLifetimes()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(ScopedClass1) && 
                                                          service.Lifetime == ServiceLifetime.Scoped));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(TransientClass1) && 
                                                          service.Lifetime == ServiceLifetime.Transient));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(SingletonClass1) && 
                                                          service.Lifetime == ServiceLifetime.Singleton));

        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(TransientClass2) &&
                                                       service.Lifetime == ServiceLifetime.Scoped));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(SingletonClass2) &&
                                                       service.Lifetime == ServiceLifetime.Transient));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(ScopedClass2) &&
                                                       service.Lifetime == ServiceLifetime.Singleton));
    }

    [Fact]
    public void AddServices_OneAssemly_CorrectServiceImplementationTypes()
    {
        // Arrange
        var services = new ServiceCollection();

        //Act
        services.AddServices(Assembly.GetAssembly(typeof(ScopedClass1)));

        //Assert
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(ScopedClass1) &&
                                                          service.ImplementationType == typeof(IScopedClass)));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(TransientClass1) &&
                                                          service.ImplementationType == typeof(ITransientClass)));
        Assert.NotNull(services.FirstOrDefault(service => service.ServiceType == typeof(SingletonClass1) &&
                                                          service.ImplementationType == typeof(ISingletonClass)));

        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(ScopedClass2) &&
                                                       service.ImplementationType == typeof(IScopedClass)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(TransientClass2) &&
                                                       service.ImplementationType == typeof(ITransientClass)));
        Assert.Null(services.FirstOrDefault(service => service.ServiceType == typeof(SingletonClass2) &&
                                                       service.ImplementationType == typeof(ISingletonClass)));
    }

    [Fact]
    public void AddServices_OneAssemly_NotRegisterIncorrectServices()
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