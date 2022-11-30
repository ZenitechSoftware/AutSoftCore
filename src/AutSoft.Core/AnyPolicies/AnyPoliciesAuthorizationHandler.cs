using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.Common.AnyPolicies;

/// <summary>
/// Evulate the policies in the <see cref="AnyPoliciesRequirement"/> with OR relationship,
/// so it is enough for a policy to pass the test
/// </summary>
public class AnyPoliciesAuthorizationHandler : AuthorizationHandler<AnyPoliciesRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initialize a new instance of the <see cref="AnyPoliciesAuthorizationHandler"/> class.
    /// </summary>
    /// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/></param>
    /// <remarks>
    /// Not possible to inject <see cref="IAuthorizationService"/>, because it would be a circular reference
    /// </remarks>
    public AnyPoliciesAuthorizationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Evulate the policies in the <see cref="AnyPoliciesRequirement"/> with OR relationship,
    /// with the help of <see cref="IAuthorizationService"/>
    /// </summary>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AnyPoliciesRequirement requirement)
    {
        var authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>();

        foreach (var policy in requirement.Policies)
        {
            var result = await authorizationService.AuthorizeAsync(context.User, context.Resource, policy);
            if (result.Succeeded)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
