namespace AutSoft.AspNetCore.Blazor.Clipboard;

/// <summary>
/// Service providing access to clipboard.
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Read text from clipboard.
    /// </summary>
    ValueTask<string> ReadTextAsync();

    /// <summary>
    /// Write text to clipboard.
    /// </summary>
    ValueTask WriteTextAsync(string text);
}
