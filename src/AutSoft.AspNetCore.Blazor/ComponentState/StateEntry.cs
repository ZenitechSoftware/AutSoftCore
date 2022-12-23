using System.Reflection;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// State of the entry.
/// </summary>
public class StateEntry
{
    /// <summary>
    /// Member metadata.
    /// </summary>
    public MemberInfo Member { get; }

    /// <summary>
    /// Value.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Constructor of the StateEntry.
    /// </summary>
    /// <param name="member">Member metadata.</param>
    /// <param name="value">Value.</param>
    public StateEntry(MemberInfo member, object value)
    {
        Member = member;
        Value = value;
    }
}
