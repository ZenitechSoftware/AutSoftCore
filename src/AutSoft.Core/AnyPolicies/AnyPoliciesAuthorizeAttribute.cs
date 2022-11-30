using Microsoft.AspNetCore.Authorization;

namespace AutSoft.Common.AnyPolicies;

/// <summary>
/// The policies, which are specified in the constructor or <see cref="Policies"/> property
/// will be evulated with OR condition with help of <see cref="AnyPoliciesAuthorizationHandler"/>
/// </summary>
/// <remarks>
/// The policies will be evulated in a new dinamically generated policy,
/// what the <see cref="AnyPoliciesPolicyProvider"/> class generate.
/// </remarks>
public class AnyPoliciesAuthorizeAttribute : AuthorizeAttribute
{
    private string[] _policies = Array.Empty<string>();

    /// <summary>
    /// The policies in OR relationship
    /// </summary>
    public string[] Policies
    {
        get => _policies;

        set
        {
            _policies = value;
            Policy = AnyPoliciesPolicyProvider.GenerateDynamicPolicy(_policies);
        }
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="AnyPoliciesAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="policies">The policies in OR relationship</param>
    public AnyPoliciesAuthorizeAttribute(params string[] policies)
    {
        Policies = policies;
    }
}
