namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// Attribute for the components properties and fields to preserve their states.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PreserveStateAttribute : Attribute
{
}
