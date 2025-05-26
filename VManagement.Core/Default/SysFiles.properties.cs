using VManagement.Commons.Entities.Attributes;
using VManagement.Commons.Utility;
using VManagement.Core.Business;
using VManagement.Core.Entities;

namespace VManagement.Core.Default
{
    [EntityName("SYS_FILES")]
    public partial class SysFiles : BusinessEntity<SysFiles>
    {
        public string? Name
        {
            get
            {
                if (Fields[FieldNames.Name] == null)
                    return null;

                return Fields[FieldNames.Name].SafeToString();
            }
            set
            {
                Fields[FieldNames.Name] = value;
            }
        }

        public string? Extension
        {
            get
            {
                if (Fields[FieldNames.Extension] == null)
                    return null;

                return Fields[FieldNames.Extension].SafeToString();
            }
            set
            {
                Fields[FieldNames.Extension] = value;
            }
        }

        public string? FullPath
        {
            get
            {
                if (Fields[FieldNames.FullPath] == null)
                    return null;

                return Fields[FieldNames.FullPath].SafeToString();
            }
            set
            {
                Fields[FieldNames.FullPath] = value;
            }
        }

        public SysFilesFileTypeListField? Type
        {
            get
            {
                return Fields[FieldNames.Type].SafeToInt32();
            }
            set
            {
                Fields[FieldNames.Type] = value?.Index;
            }
        }

        public DateTime? InsertDate
        {
            get
            {
                if (Fields[FieldNames.InsertDate] == null)
                    return null;

                return Fields[FieldNames.InsertDate].SafeToDateTime();   
            }
            set
            {
                Fields[FieldNames.InsertDate] = value;
            }
        }

        public DateTime? UpdateDate
        {
            get
            {
                if (Fields[FieldNames.UpdateDate] == null)
                    return null;

                return Fields[FieldNames.UpdateDate].SafeToDateTime();
            }
            set
            {
                Fields[FieldNames.UpdateDate] = value;
            }
        }

        public string? EntityName
        {
            get
            {
                if (Fields[FieldNames.EntityName] == null)
                    return null;

                return Fields[FieldNames.EntityName].SafeToString();
            }
            set
            {
                Fields[FieldNames.EntityName] = value;
            }
        }

        public string? EntityField
        {
            get
            {
                if (Fields[FieldNames.EntityField] == null)
                    return null;

                return Fields[FieldNames.EntityField].SafeToString();
            }
            set
            {
                Fields[FieldNames.EntityField] = value;
            }
        }

        public long? EntityId
        {
            get
            {
                if (Fields[FieldNames.EntityId] == null)
                    return null;

                return Fields[FieldNames.EntityId].SafeToInt32();
            }
            set
            {
                Fields[FieldNames.EntityId] = value;
            }
        }

        public static class FieldNames
        {
            public const string Name = "NAME";
            public const string Extension = "EXTENSION";
            public const string FullPath = "FULLPATH";
            public const string Type = "TYPE";
            public const string InsertDate = "INSERTDATE";
            public const string UpdateDate = "UPDATEDATE";
            public const string EntityName = "ENTITYNAME";
            public const string EntityField = "ENTITYFIELD";
            public const string EntityId = "ENTITYID";
        }
    }

    public class SysFilesFileTypeListField : ListField
    {
        public static readonly List<SysFilesFileTypeListField> Items = new List<SysFilesFileTypeListField>();

        public static readonly SysFilesFileTypeListField ItemPDF   = new() { Index = 1, Description = "PDF" };
        public static readonly SysFilesFileTypeListField ItemDOCX  = new() { Index = 2, Description = "DOCX" };
        public static readonly SysFilesFileTypeListField ItemXLSX  = new() { Index = 3, Description = "XLSX" };
        public static readonly SysFilesFileTypeListField ItemTXT   = new() { Index = 4, Description = "TXT" };
        public static readonly SysFilesFileTypeListField ItemOther = new() { Index = 5, Description = "Other" };

        static SysFilesFileTypeListField()
        {
            Items.Add(ItemPDF);
            Items.Add(ItemDOCX);
            Items.Add(ItemXLSX);
            Items.Add(ItemTXT);
            Items.Add(ItemOther);
        }

        public static implicit operator int(SysFilesFileTypeListField item) => item.Index;
        public static implicit operator SysFilesFileTypeListField?(int index) => ByIndex(index);

        public static SysFilesFileTypeListField? ByIndex(int index)
            => Items.FirstOrDefault(item => item.Index == index);
    }
}
