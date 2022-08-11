using AutSoft.DbScaffolding.EntityAbstractions;
using EntityFrameworkCore.Scaffolding.Handlebars;
using System;
using System.Collections.Generic;

namespace AutSoft.DbScaffolding
{
    public class DbScaffoldingOptions
    {
        public Dictionary<DbColumn, Type> ColumnToEnumDictionary { get; set; } = new Dictionary<DbColumn, Type>();

        public Dictionary<DbColumn, Type> ColumnToSpatialTypeDictionary { get; set; } = new Dictionary<DbColumn, Type>();

        public HandlebarsScaffoldingOptions HandlebarsScaffoldingOptions { get; set; } = new HandlebarsScaffoldingOptions();

        public InterfaceProperties InterfaceProperties { get; set; } = new InterfaceProperties();
    }
}
