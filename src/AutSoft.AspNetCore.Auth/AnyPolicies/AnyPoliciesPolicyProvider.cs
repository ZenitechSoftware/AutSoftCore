using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AutSoft.AspNetCore.Auth.AnyPolicies;

/// <summary>
/// Dynamically generate policies based on <see cref="AnyPoliciesAuthorizeAttribute"/>,
/// that will contain a <see cref="AnyPoliciesRequirement"/> requirement.
/// For anything not <see cref="AnyPoliciesAuthorizeAttribute"/>,
/// the default policy provider (<see cref="DefaultAuthorizationPolicyProvider"/>) will return the policy.
/// </summary>
public class AnyPoliciesPolicyProvider : IAuthorizationPolicyProvider
{
    /// <summary>
    /// Prefix of dinamically generated policy's name
    /// </summary>
    public const string PolicyPrefix = "AnyPolicies_";

    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    /// <summary>
    /// Initialize a new instance of <see cref="AnyPoliciesPolicyProvider"/> class.
    /// </summary>
    /// <param name="options">Options of provider</param>
    public AnyPoliciesPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    /// <summary>
    /// Return with the name of policies from the name of dynamic policy
    /// </summary>
    /// <param name="dynamicPolicyName">The name of dinamic policy</param>
    /// <returns>The name of policies</returns>
    public static IEnumerable<string> GetPolicyNamesFromDynamicPolicy(string dynamicPolicyName)
    {
        return dynamicPolicyName[PolicyPrefix.Length..].Split(',');
    }

    /// <summary>
    /// Generate a dynamic policy with the OR combine of policies
    /// </summary>
    /// <param name="policies">The policies to be combine</param>
    /// <returns>The dynamic policy</returns>
    public static string GenerateDynamicPolicy(IEnumerable<string> policies)
    {
        return PolicyPrefix + string.Join(",", policies);
    }

    /// <summary>
    /// Return with the default authorization policy
    /// </summary>
    /// <returns>The default authorization policy</returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    /// <summary>
    /// Return with the authorization policy with the specified policy name
    /// </summary>
    /// <param name="policyName">The specified policy name</param>
    /// <returns>The authorization policy with the specified name</returns>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var policies = GetPolicyNamesFromDynamicPolicy(policyName).ToArray();
            var policy = new AuthorizationPolicyBuilder().AddRequirements(new AnyPoliciesRequirement(policies));
            return Task.FromResult((AuthorizationPolicy?)policy.Build());
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    /// <summary>
    /// Return with the fallback policy
    /// </summary>
    /// <returns>The fallback policy</returns>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}
