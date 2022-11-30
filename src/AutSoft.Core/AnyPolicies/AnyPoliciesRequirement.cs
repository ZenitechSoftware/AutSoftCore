using Microsoft.AspNetCore.Authorization;

namespace AutSoft.Common.AnyPolicies;

/// <summary>
/// Requirement of wanted to combine OR authorizations
/// </summary>
public class AnyPoliciesRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Initialize a new instance of <see cref="AnyPoliciesRequirement"/> class.
    /// </summary>
    /// <param name="policies">Array of policies to be combined</param>
    public AnyPoliciesRequirement(params string[] policies)
    {
        Policies = policies;
    }

    /// <summary>
    /// Policies to be combine OR
    /// </summary>
    public IEnumerable<string> Policies { get; }
}
