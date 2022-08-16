using EntityFrameworkCore.Scaffolding.Handlebars;

using System;
using System.Collections.Generic;

namespace AutSoft.DbScaffolding.Configuration;

/// <summary>
/// Configuration of the scaffolding design time process
/// </summary>
public class DbScaffoldingOptions
{
    /// <summary>
    /// Gets or sets enum configurations for columns
    /// </summary>
    public Dictionary<DbColumn, Type> ColumnToEnumDictionary { get; set; } = new Dictionary<DbColumn, Type>();

    /// <summary>
    /// Gets or sets spatial type configurations for columns
    /// </summary>
    public Dictionary<DbColumn, Type> ColumnToSpatialTypeDictionary { get; set; } = new Dictionary<DbColumn, Type>();

    /// <summary>
    /// Gets or sets handlebar generator's configuration
    /// </summary>
    public HandlebarsScaffoldingOptions HandlebarsScaffoldingOptions { get; set; } = new HandlebarsScaffoldingOptions();

    /// <summary>
    /// Gets or sets configuration of generated misc interfaces
    /// </summary>
    public InterfaceProperties InterfaceProperties { get; set; } = new InterfaceProperties();
}
