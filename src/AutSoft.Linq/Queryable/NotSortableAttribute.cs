namespace AutSoft.Linq.Queryable;

/// <summary>
/// Indicates if a property is not sortable by <see cref="OrderByExtensions"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NotSortableAttribute : Attribute
{
}
