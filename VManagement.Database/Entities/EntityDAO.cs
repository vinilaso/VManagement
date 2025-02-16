using Microsoft.Data.SqlClient;
using VManagement.Commons.Interfaces;
using VManagement.Database.Connection;
using VManagement.Database.SqlClauses;

namespace VManagement.Database.Entities
{
    public abstract class EntityDAO<T> where T : IEntity, new()
    {
        private CommandBuilder? _commandBuilder;
        private IEntity _entity;

        public IEntity Entity => _entity;

        public EntityDAO(IEntity entity)
        {
            _entity = entity;
        }

        internal void Save()
        {
            var transaction = TransactionScope.Current;

            if (transaction != null)
            {
                var command = transaction.Connection.CreateCommand();

                var cmdBuilder = new CommandBuilder(_entity, command);
                command.CommandText = cmdBuilder.InsertClause;

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    _entity.Id = reader.GetInt64(0);
                }
            }
            else
            {
                using VManagementConnection connection = new VManagementConnection();
                var command = connection.CreateCommand();

                var cmdBuilder = new CommandBuilder(_entity, command);
                command.CommandText = cmdBuilder.InsertClause;

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    _entity.Id = reader.GetInt64(0);
                }
            }
        }
    }
}
