using Microsoft.JSInterop;

using System.Globalization;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// Extension methods for <see cref="IJSRuntime">IJSRuntime</see>.
/// </summary>
public static class JSRuntimeExtensions
{
    /// <summary>
    /// Gets the navigation state time key.
    /// </summary>
    public static async ValueTask<string> GetNavStateTimeKeyAsync(this IJSRuntime jsRuntime) =>
        (await jsRuntime.InvokeAsync<long>("get_nav_state_time_key")).ToString(CultureInfo.InvariantCulture);
}
