namespace EndpointConfigurator;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("ENDPOINTCONFIG_USAGES")]
public sealed class EndpointConfigAttribute : Attribute
{
    [System.Runtime.CompilerServices.CompilerGenerated]
    public EndpointConfigAttribute()
    {
    }
}
