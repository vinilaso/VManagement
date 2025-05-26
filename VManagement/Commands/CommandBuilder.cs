using System.Runtime.CompilerServices;
using VManagement.Commons.Entities;
using VManagement.Database.Clauses;
using static VManagement.Database.Commands.CommandConstants;

[assembly: InternalsVisibleTo("VManagement.Database.Tests")]
namespace VManagement.Database.Commands
{
    internal class CommandBuilder
    {
        private const string ALIAS = "A";
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

        internal string BuildSelectClause()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_entity.Fields == null || !_entity.Fields.Any())
                throw new InvalidOperationException("The entity has no fields to retrieve.");

            string fieldsToRetrieve = string.Join(",", _entity.Fields.Select(field => field.WithAlias(ALIAS)));

            return string.Format(SELECT_LAYOUT, fieldsToRetrieve, $"{_entity.Schema.EntityName} {ALIAS}", _restriction);
        }

        internal string BuildUpdateClause()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            if (_entity.Fields == null || !_entity.Fields.Any())
                throw new InvalidOperationException("The entity has no fields to update.");

            string fieldsToUpdate = string.Join(",", _entity.Fields
                .Where(field => field.Name != "ID")
                .Select(field => $"{field.Name} = @{field.Name}"));

            return string.Format(UPDATE_LAYOUT, $"{_entity.Schema.EntityName}", fieldsToUpdate, _restriction);
        }

        internal string BuildInsertClause()
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

            return string.Format(INSERT_LAYOUT, _entity.Schema.EntityName, fieldsToInsert, valuesToInsert);
        }

        internal string BuildDeleteClause()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            return string.Format(DELETE_LAYOUT, $"{_entity.Schema.EntityName}", _restriction);
        }

        internal string BuildExistsClause()
        {
            if (_entity == null)
                throw new ArgumentNullException(nameof(_entity));

            return string.Format(EXISTS_LAYOUT, $"{_entity.Schema.EntityName} {ALIAS}", _restriction);
        }

        internal static string FormatQuery(string query, Restriction restriction)
        {
            return $"SELECT * FROM ({query}) A {restriction}"; 
        }
    }
}
