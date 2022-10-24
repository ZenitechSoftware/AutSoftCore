#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace AutSoft.DbScaffolding.EntityAbstractions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Configure ISoftDeletableEntity and IAuditableEntity interface generation
/// </summary>
public class InterfaceProperties
{
    /// <summary>
    /// Gets or sets generated type's namespace
    /// </summary>
    public string InterfaceNameSpace { get; set; } = typeof(InterfaceProperties).Namespace;

    /// <summary>
    /// Gets or sets interface name for indicate soft delete
    /// Default: ISoftDeletableEntity
    /// </summary>
    public string SoftDeletableInterfaceName { get; set; } = "ISoftDeletableEntity";

    /// <summary>
    /// Gets or sets interface name for audit information
    /// Default: IAuditableEntity
    /// </summary>
    public string AuditableInterfaceName { get; set; } = "IAuditableEntity";

    /// <summary>
    /// Gets or sets property name for indicate soft delete
    /// Default: this property's name
    /// </summary>
    public string IsDeleted { get; set; } = nameof(IsDeleted);

    /// <summary>
    /// Gets or sets property name for "created at" audit information
    /// Default: this property's name
    /// </summary>
    public string CreatedAt { get; set; } = nameof(CreatedAt);

    /// <summary>
    /// Gets or sets property name for "created by" audit information
    /// Default: this property's name
    /// </summary>
    public string CreatedBy { get; set; } = nameof(CreatedBy);

    /// <summary>
    /// Gets or sets property name for "last modified at" audit information
    /// Default: this property's name
    /// </summary>
    public string LastModAt { get; set; } = nameof(LastModAt);

    /// <summary>
    /// Gets or sets property name for "last modified by" audit information
    /// Default: this property's name
    /// </summary>
    public string LastModBy { get; set; } = nameof(LastModBy);
}
