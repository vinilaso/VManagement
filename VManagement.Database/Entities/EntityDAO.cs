using Microsoft.Data.SqlClient;
using System.Data;
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

        public void Save()
        {
            var transactionScope = TransactionScope.Current;

            if (transactionScope != null)
            {
                var command = transactionScope.Connection.CreateCommand();
                command.Transaction = transactionScope.Transaction;

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

        public void Update()
        {
            var transactionScope = TransactionScope.Current;

            if (transactionScope != null)
            {
                var command = transactionScope.Connection.CreateCommand();
                command.Transaction = transactionScope.Transaction;

                var commandBuilder = new CommandBuilder(_entity, command)
                {
                    Restriction = Restriction.FromId(_entity.Id)
                };

                command.CommandText = commandBuilder.UpdateClause;
                command.ExecuteNonQuery();
            }
            else
            {
                using VManagementConnection connection = new VManagementConnection();
                var command = connection.CreateCommand();

                var commandBuilder = new CommandBuilder(_entity, command)
                {
                    Restriction = Restriction.FromId(_entity.Id)
                };

                command.CommandText= commandBuilder.UpdateClause;
                command.ExecuteNonQuery();
            }

            ReloadEntity(_entity);
        }

        public void Delete()
        {
            var transactionScope = TransactionScope.Current;

            if (transactionScope != null)
            {
                var command = transactionScope.Connection.CreateCommand();
                command.Transaction = transactionScope.Transaction;

                var commandBuilder = new CommandBuilder(_entity, command)
                {
                    Restriction = Restriction.FromId(_entity.Id)
                };

                command.CommandText = commandBuilder.DeleteClause;
                command.ExecuteNonQuery();
            }
            else
            {
                using VManagementConnection connection = new VManagementConnection();
                var command = connection.CreateCommand();

                var commandBuilder = new CommandBuilder(_entity, command)
                {
                    Restriction = Restriction.FromId(_entity.Id)
                };

                command.CommandText = commandBuilder.ToString();
                command.ExecuteNonQuery();
            }
        }

        private static T CreateTypeInstance()
        {
            T instance = new T();

            foreach (var field in instance.AllFieldNames())
                instance.Fields.Add(field, null);

            return instance;
        }

        public static void ReloadEntity(IEntity entity)
        {
            var reloadedEntity = GetFirstOrDefault(Restriction.FromId(entity.Id));

            if (reloadedEntity != null)
            {
                entity = reloadedEntity;
            }
            else
            {
                throw new OperationCanceledException($"There was no entity in the database with {entity.Id} as ID.");
            }
        }

        public static T? GetFirstOrDefault(Restriction restriction)
        {
            T entity = CreateTypeInstance();

            using (var connection = new VManagementConnection())
            {
                var command = connection.CreateCommand();

                var commandBuilder = new CommandBuilder(entity, command)
                {
                    Restriction = restriction
                };

                command.CommandText = commandBuilder.SelectClause;

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    foreach (string field in entity.AllFieldNames())
                    {
                        var initialField = new FieldValue()
                        {
                            Name = field
                        };

                        initialField.SetValue(reader.GetValue(field));

                        entity[field] = initialField as IField;
                    }

                    return entity;
                }

                return default;
            }
        }

        public static ICollection<T> GetMany(Restriction restriction)
        {
            T entity = CreateTypeInstance();

            using (var connection = new VManagementConnection())
            {
                var command = connection.CreateCommand();

                var commandBuilder = new CommandBuilder(entity, command)
                { 
                    Restriction = restriction
                };

                command.CommandText= commandBuilder.SelectClause;

                using SqlDataReader reader = command.ExecuteReader();

                ICollection<T> collection = new List<T>();

                while (reader.Read())
                {
                    entity = CreateTypeInstance();

                    foreach (string field in entity.AllFieldNames())
                    {
                        entity.Fields[field] = reader.GetValue(field);
                    }

                    collection.Add(entity);
                }

                return collection;
            }
        }
    }
}
