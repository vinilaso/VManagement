namespace VManagement.Commons.Enum
{
    public enum EntityState
    {
        Created,
        Loaded,
        Editing,
        Deleted
    }

    public static class EntityStateExtensions
    {
        public static bool IsCreating(this EntityState entityState)
        {
            return entityState == EntityState.Created;
        }

        public static bool IsLoaded(this EntityState entityState)
        {
            return entityState == EntityState.Loaded;
        }

        public static bool IsEditing(this EntityState entityState)
        {
            return entityState == EntityState.Editing;
        }

        public static bool IsDeleted(this EntityState entityState)
        {
            return entityState == EntityState.Deleted;
        }
    }
}
