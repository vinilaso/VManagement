namespace VManagement.Analyzers.DTO
{
    internal readonly struct ClassInfo
    {
        public readonly string ClassNamespace;
        public readonly string ClassName;
        public readonly string EntityName;

        public ClassInfo(string classNamespace, string className, string entityName)
        {
            ClassNamespace = classNamespace;
            ClassName = className;
            EntityName = entityName;
        }
    }
}
