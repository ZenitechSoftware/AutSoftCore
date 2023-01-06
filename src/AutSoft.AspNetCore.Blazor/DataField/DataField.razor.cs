using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.DataField;

/// <summary>
/// Data field
/// </summary>
public partial class DataField
{
    /// <summary>
    /// Label.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = default!;

    /// <summary>
    /// Child content.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Label xs.
    /// </summary>
    [Parameter]
    public int LabelXs { get; set; }

    /// <summary>
    /// Label sm.
    /// </summary>
    [Parameter]
    public int LabelSm { get; set; }

    /// <summary>
    /// Label md.
    /// </summary>
    [Parameter]
    public int LabelMd { get; set; }

    /// <summary>
    /// Label lg.
    /// </summary>
    [Parameter]
    public int LabelLg { get; set; }

    /// <summary>
    /// Label xl.
    /// </summary>
    [Parameter]
    public int LabelXl { get; set; }

    /// <summary>
    /// Label xxl.
    /// </summary>
    [Parameter]
    public int LabelXxl { get; set; }

    /// <summary>
    /// Value xs.
    /// </summary>
    [Parameter]
    public int ValueXs { get; set; }

    /// <summary>
    /// Value sm.
    /// </summary>
    [Parameter]
    public int ValueSm { get; set; }

    /// <summary>
    /// Value md.
    /// </summary>
    [Parameter]
    public int ValueMd { get; set; }

    /// <summary>
    /// Value lg.
    /// </summary>
    [Parameter]
    public int ValueLg { get; set; }

    /// <summary>
    /// Value xl.
    /// </summary>
    [Parameter]
    public int ValueXl { get; set; }

    /// <summary>
    /// Value xxl.
    /// </summary>
    [Parameter]
    public int ValueXxl { get; set; }
}
