# AutoDiInjector

## About
This is a lightweight and fast extension auto-registers all public classes to their interfaces or base class.

## Usage
You need to mark the class from one of those attributes, which contain a corresponding lifetime. Also, you can pass the interface or base class type in the constructor to implement it.

`[TransientServiceAttribute]`

`[ScopedServiceAttribute]`

`[SingletonServiceAttribute]`


Then in your `ConfigureServices` method in Startup.cs or Program.cs, if you use the dotnet 6, call the `AddServices` method on `IServiceCollection`.

`public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)`

Or overload
	
`public static IServiceCollection AddServices(this IServiceCollection services, Func<Type, bool> predicate, params Assembly[] assemblies)`

Where you can pass the predicate, which is used to select the services.