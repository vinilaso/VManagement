using System.Runtime.CompilerServices;
using VManagement.Commons.Entities;
using VManagement.Database.Clauses;
using VManagement.Database.Connection;
using VManagement.Database.Entities;
using static VManagement.Database.Commands.CommandConstants;

[assembly: InternalsVisibleTo("VManagement.Database.Tests")]
namespace VManagement.Database.Commands
{
    internal class CommandBuilder
    {
        private const string ALIAS = "A";
        private readonly VManagementConnection? _connection;
        private readonly CoreEntity _entity;
        private readonly Restriction _restriction = Restriction.Empty;

        internal CommandBuilder(CoreEntity entity)
        {
            _entity = entity;
        }

        internal CommandBuilder(CoreEntity entity, Restriction restriction)
        {
            _entity = entity;
            _restriction = restriction;
        }
        
        internal CommandBuilder(CoreEntity entity, Restriction restriction, VManagementConnection connection)
        {
            _entity = entity;
            _restriction = restriction;
            _connection = connection;
        }

        internal VManagementCommand BuildSelectCommand()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_entity.Fields == null || !_entity.Fields.Any())
                throw new InvalidOperationException("The entity has no fields to retrieve.");

            string fieldsToRetrieve = string.Join(",", _entity.Fields.Select(field => field.WithAlias(ALIAS)));

            if (_connection == null)
                throw new ArgumentNullException(nameof(_connection));

            VManagementCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(SELECT_LAYOUT, fieldsToRetrieve, $"{_entity.Schema.EntityName} {ALIAS}", _restriction);

            if (_restriction != null)
                command.SetParameters(_restriction.Parameters);

            return command;
        }

        internal VManagementCommand BuildUpdateCommand()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_entity.Fields == null || !_entity.Fields.Any())
                throw new InvalidOperationException("The entity has no fields to update.");

            string fieldsToUpdate = string.Join(",", _entity.Fields
                .Where(field => field.Name != "ID")
                .Select(field => $"{field.Name} = @{field.Name}"));

            if (_connection == null)
                throw new ArgumentNullException(nameof(_connection));

            Restriction updateRestriction = Restriction.FromId(_entity.Id, false);
            VManagementCommand command = _connection.CreateCommand();

            command.CommandText = string.Format(UPDATE_LAYOUT, _entity.Schema.EntityName, fieldsToUpdate, updateRestriction);
            command.SetParameters(_entity);
            command.SetParameters(updateRestriction.Parameters);

            return command;
        }

        internal VManagementCommand BuildInsertCommand()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_entity.Fields == null || !_entity.Fields.Any())
                throw new InvalidOperationException("The entity has no fields to insert.");

            string fieldsToInsert = string.Join(",", _entity.Fields
                .Where(field => field.Name != "ID")
                .Select(field => field.Name));

            string valuesToInsert = string.Join(",", _entity.Fields
                .Where(field => field.Name != "ID")
                .Select(field => $"@{field.Name}"));

            if (_connection == null)
                throw new ArgumentNullException(nameof(_connection));

            VManagementCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(INSERT_LAYOUT, _entity.Schema.EntityName, fieldsToInsert, valuesToInsert);
            command.SetParameters(_entity);

            return command;
        }

        internal VManagementCommand BuildDeleteCommand()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_connection == null)
                throw new ArgumentNullException(nameof(_connection));

            VManagementCommand command = _connection.CreateCommand();
            Restriction deleteRestriction = Restriction.FromId(_entity.Id, false);
            command.CommandText = string.Format(DELETE_LAYOUT, _entity.Schema.EntityName, deleteRestriction);
            command.SetParameters(deleteRestriction.Parameters);

            return command;
        }

        internal VManagementCommand BuildExistsCommand()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_connection == null)
                throw new ArgumentNullException(nameof(_connection));

            VManagementCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(EXISTS_LAYOUT, $"{_entity.Schema.EntityName} {ALIAS}", _restriction);
            command.SetParameters(_restriction.Parameters);

            return command;
        }

        internal static string FormatQuery(string query, Restriction restriction)
        {
            return $"SELECT * FROM ({query}) A {restriction}"; 
        }
    }
}
