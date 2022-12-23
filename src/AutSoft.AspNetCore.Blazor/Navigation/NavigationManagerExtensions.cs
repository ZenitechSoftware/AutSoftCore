using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.Navigation;

/// <summary>
/// Extension methods for <see cref="NavigationManager">NavigationManager</see>.
/// </summary>
public static class NavigationManagerExtensions
{
    /// <summary>
    /// Go back navigation key.
    /// </summary>
    public const string GoBackParameterKey = "goback";

    private static readonly Dictionary<string, object> Parameters = new();

    /// <summary>
    /// Add navigation parameter.
    /// </summary>
    public static void AddParameter(this NavigationManager _, string key, object parameter)
    {
        Parameters[key] = parameter;
    }

    /// <summary>
    /// Add navigate back parameter.
    /// </summary>
    public static void AddGoBackParameter(this NavigationManager _, object parameter)
    {
        Parameters[GoBackParameterKey] = parameter;
    }

    /// <summary>
    /// Navigate to the URI with the specified parameter.
    /// </summary>
    public static void NavigateToWithParameter(this NavigationManager navigationManager, string uri, object parameter, bool forceLoad = false, bool replace = false)
    {
        navigationManager.AddParameter(GetUriWithoutQuery(uri), parameter);
        navigationManager.NavigateTo(uri, forceLoad, replace);
    }

    /// <summary>
    /// Gets the URI navigation parameter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method")]
    public static T? RetrieveParameter<T>(this NavigationManager navigationManager, string uri)
    {
        Parameters.Remove(GetUriWithoutQuery(uri), out var result);
        return (T?)result;
    }

    /// <summary>
    /// Tries to get the navigate back parameter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method")]
    public static T? TryRetrieveGoBackParameter<T>(this NavigationManager navigationManager)
    {
        if (Parameters.TryGetValue(GoBackParameterKey, out var result) && result is T t)
        {
            Parameters.Remove(GoBackParameterKey);
            return t;
        }

        return default;
    }

    private static string GetUriWithoutQuery(string uri)
    {
        return uri.Split("?").First();
    }
}
