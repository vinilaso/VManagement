using VManagement.Analyzers.DTO;

namespace VManagement.Analyzers.Helpers
{
    internal class EntityPropertyGeneratorHelper
    {
        private const string _propertyModel = @"
        [EntityColumnName(""<<COLUMNNAME>>"")]
        public <<DOTNETTYPE>> <<DOTNETNAME>>
        {
            get => Fields[""<<COLUMNNAME>>""].<<CONVERSIONMETHOD>>;
            set => Fields[""<<COLUMNNAME>>""] = value;
        }
";

        private const string _classModel = @"
using System;
using VManagement.Commons.Utility;
using VManagement.Commons.Entities.Attributes;

namespace <<NAMESPACE>>
{
    public partial class <<CLASS>>
    {
        <<PROPERTIES>>
    }
}
";

        internal static string GetFormattedClass(ClassInfo classInfo, EntityMetadata entityMetadata)
        {


            return _classModel
                .Replace("<<NAMESPACE>>", classInfo.ClassNamespace)
                .Replace("<<CLASS>>", classInfo.ClassName)
                .Replace("<<PROPERTIES>>", string.Join(Environment.NewLine, entityMetadata.Columns.Select(GetFormattedProperty)));
        }

        internal static string GetFormattedProperty(EntityColumnMetadata columnMetadata)
        {
            return _propertyModel
                .Replace("<<COLUMNNAME>>", columnMetadata.ColumnName)
                .Replace("<<DOTNETTYPE>>", columnMetadata.DotNetPropertyType)
                .Replace("<<DOTNETNAME>>", columnMetadata.DotNetPropertyName)
                .Replace("<<CONVERSIONMETHOD>>", columnMetadata.ConversionMethod);
        }
    }
}
