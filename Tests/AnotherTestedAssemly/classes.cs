using AutoDIInjector.Attributes;

namespace AnotherTestedAssemly;



[TransientService(typeof(ITransientClass))] public class TransientClass1 : ITransientClass { }
[TransientService] public class TransientClass2 { }

//////////////////////////////////////////////////

[ScopedService(typeof(IScopedClass))] public class ScopedClass1 : IScopedClass { }
[ScopedService] public class ScopedClass2 { }

//////////////////////////////////////////////////

[SingletonService(typeof(ISingletonClass))] public class SingletonClass1 : ISingletonClass { }
[SingletonService] public class SingletonClass2 { }

//////////////////////////////////////////////////
///
public class DefaultClass { }
[SingletonService] internal class DefaultInternalClass { }
[SingletonService] public abstract class DefaultAbstractClass { }
[SingletonService] public abstract class DefaultAbstractGenericClass<T> { }
