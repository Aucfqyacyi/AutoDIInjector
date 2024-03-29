﻿using Microsoft.Extensions.DependencyInjection;

namespace AutoDIInjector.Attributes;

public class TransientServiceAttribute : ServiceAttribute
{
    public TransientServiceAttribute(Type? interfaceType = null) : base(ServiceLifetime.Transient, interfaceType)
    {
    }

}

public class TransientServiceAttribute<TInterface> : TransientServiceAttribute
{
    public TransientServiceAttribute() : base(typeof(TInterface))
    {
    }
}