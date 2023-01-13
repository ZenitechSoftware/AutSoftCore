using System.Text;

namespace AutSoft.AspNetCore.Blazor.ErrorHandling;

/// <summary>
/// Display error.
/// </summary>
public class DisplayError
{
    /// <summary>
    /// Constructor of the DisplayError.
    /// </summary>
    /// <param name="title">Title.</param>
    /// <param name="details">Details.</param>
    /// <param name="technicalDetails">Technical details.</param>
    /// <param name="validationErrors">Validation errors.</param>
    /// <param name="correlationId">Correlation id.</param>
    public DisplayError(string title, string? details = null, string? technicalDetails = null, Dictionary<string, List<string>>? validationErrors = null, string? correlationId = null)
    {
        Title = title;
        Details = details ?? string.Empty;
        TechnicalDetails = technicalDetails ?? string.Empty;
        ValidationErrors = validationErrors ?? new Dictionary<string, List<string>>();
        CorrelationId = correlationId;
    }

    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Details.
    /// </summary>
    public string Details { get; }

    /// <summary>
    /// Technical details.
    /// </summary>
    public string TechnicalDetails { get; }

    /// <summary>
    /// Validation errors.
    /// </summary>
    public Dictionary<string, List<string>> ValidationErrors { get; }

    /// <summary>
    /// Validation errors in displayable format.
    /// </summary>
    public string DisplayValidationErrors
        => ValidationErrors.Count > 0 ? string.Join('\n', ValidationErrors.SelectMany(v => v.Value)) : string.Empty;

    /// <summary>
    /// Correlation id.
    /// </summary>
    public string? CorrelationId { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        StringBuilder builder = new();

        if (!string.IsNullOrEmpty(CorrelationId))
            builder.AppendLine(CorrelationId);

        if (!string.IsNullOrEmpty(Title))
            builder.AppendLine(Title);

        if (!string.IsNullOrEmpty(Details))
            builder.AppendLine(Details);

        if (ValidationErrors?.Any() == true)
            builder.AppendLine(string.Join("\n", ValidationErrors.Select(c => $"{c.Key}: {string.Join(",", c.Value)}")));

        if (!string.IsNullOrEmpty(TechnicalDetails))
            builder.AppendLine(TechnicalDetails);

        return builder.ToString();
    }
}
