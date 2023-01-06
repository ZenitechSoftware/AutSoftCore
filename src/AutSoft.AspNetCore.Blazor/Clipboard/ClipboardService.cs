using Microsoft.JSInterop;

namespace AutSoft.AspNetCore.Blazor.Clipboard;

/// <inheritdoc />
public class ClipboardService : IClipboardService
{
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Constructor of the ClipboardService.
    /// </summary>
    /// <param name="jsRuntime">Javascript runtime.</param>
    public ClipboardService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <inheritdoc />
    public ValueTask<string> ReadTextAsync()
    {
        return _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }

    /// <inheritdoc />
    public ValueTask WriteTextAsync(string text)
    {
        return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}
