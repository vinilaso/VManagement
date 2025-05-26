using Microsoft.IdentityModel.Tokens;
using VManagement.Commons.Entities;
using VManagement.Commons.Enum;
using VManagement.Commons.Utility;
using VManagement.Database;
using VManagement.Database.Clauses;

namespace VManagement.Core.Default
{
    public partial class SysFiles
    {
        public static async Task<byte[]> GetContent(CoreEntity entity, string fieldName)
        {
            Restriction restriction = new Restriction("A.ENTITYNAME = @NAME AND A.ENTITYFIELD = @FIELD AND A.ENTITYID = @ID");
            restriction.Parameters.Add("@NAME", entity.Schema.EntityName);
            restriction.Parameters.Add("@FIELD", fieldName);
            restriction.Parameters.Add("@ID", entity.Id);

            SysFiles? file = GetFirstOrDefault(restriction);

            if (file is null)
                throw new ArgumentException("The entity has no files attached.");

            return await File.ReadAllBytesAsync(file.FullPath ?? string.Empty);
        }

        protected override void Saving()
        {
            ValidateFile();

            if (IsCreating)
            {
                CreateInternalFile();
            }

            SetExtension();

            SetFileName();

            SetFileType();

            SetInsertDate();

            base.Saving();
        }

        protected override void Editing()
        {
            SetUpdateDate();

            UpdateFile();

            base.Editing();
        }

        protected override void Deleted()
        {
            DeleteFile();

            base.Deleted();
        }

        private void UpdateFile()
        {
            if (this[FieldNames.FullPath].Changed)
            {
                CreateInternalFile();
                File.Delete(this[FieldNames.FullPath].OriginalValue.SafeToString());
            }
            else if (this[FieldNames.Name].Changed)
            {
                File.Move(this[FieldNames.Name].OriginalValue.SafeToString(), Name);
            }
        }

        private void SetFileType()
        {
            var fileType = SysFilesFileTypeListField.Items
                .FirstOrDefault(item => item.Description == Extension?.ToUpper());

            if (fileType == null)
            {
                Type = SysFilesFileTypeListField.ItemOther;
            }
            else
            {
                Type = fileType;
            }
        }

        private void SetFileName()
        {
            Name = Path.GetFileNameWithoutExtension(FullPath);
        }

        private void CreateInternalFile()
        {
            string fileName = Path.GetFileName(FullPath!);

            var internalFilePath = Path.Combine(Security.InternalFilesPath, fileName);

            if (File.Exists(internalFilePath))
                File.Delete(internalFilePath);

            using FileStream externalFile = File.OpenRead(FullPath!);
            using FileStream internalFile = File.Create(internalFilePath);

            externalFile.CopyTo(internalFile);

            FullPath = internalFilePath;
        }

        private void ValidateFile()
        {
            if (!File.Exists(FullPath))
                throw new OperationCanceledException($"The path {FullPath.SafeToString()} is invalid.");
        }

        private void DeleteFile()
        {
            if (!File.Exists(FullPath))
                return;

            File.Delete(FullPath);
        }

        private void SetUpdateDate()
        {
           UpdateDate = DateTime.Now;
        }

        private void SetExtension()
        {
            string extension = Path.GetExtension(FullPath ?? string.Empty);

            if (extension.IsNullOrEmpty())
                throw new OperationCanceledException("There was no extension in the file name.");

            Extension = extension;
        }

        private void SetInsertDate()
        {
            if (State == EntityState.New)
                InsertDate = DateTime.Now;
        }
    }
}
