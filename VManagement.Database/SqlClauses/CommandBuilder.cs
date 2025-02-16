using Microsoft.Data.SqlClient;
using VManagement.Commons.Interfaces;
using VManagement.Commons.Utility;
using VManagement.Database.Utility;

namespace VManagement.Database.SqlClauses
{
    internal class CommandBuilder
    {
        private const string ALIAS = "A";
        private IEntity _entity;
        private SqlCommand? _command;

        internal Restriction Restriction { get; set; } = Restriction.Empty;

        internal string SelectClause => BuildSelectClause();
        internal string InsertClause => BuildInsertClause();

        internal CommandBuilder(IEntity entity)
        {
            _entity = entity;
        }

        internal CommandBuilder(IEntity entity, SqlCommand command)
        {
            _entity = entity;
            _command = command;
        }

        private string BuildSelectClause()
        {
            ValidateInstanceCommand();

            var builder = new DelimitedStringBuilder(" ");

            builder.Append("SELECT")
                   .AppendJoin(", ", _entity.AllFieldNames())
                   .Append("FROM")
                   .Append(_entity.TableName)
                   .Append(ALIAS);

            if (Restriction != Restriction.Empty)
            {
                builder.Append(Restriction.ToString());
            }

            Restriction.SetParameters(_command!);
            return builder.ToString();
        }

        private string BuildInsertClause()
        {
            ValidateInstanceCommand();

            var builder = new DelimitedStringBuilder(" ");
            var fieldNames = _entity.AllFieldNames(ignoreId: true);

            builder.Append("INSERT INTO")
                   .Append(_entity.TableName)

                   .OpenParenthesis()
                   .AppendJoin(", ", fieldNames)
                   .CloseParenthesis()

                   .Append("OUTPUT INSERTED.ID")
                   .Append("VALUES")

                   .OpenParenthesis()
                   .AppendJoin(", ", fieldNames.Select(name => name.AsParameter()))
                   .CloseParenthesis();

            Restriction restriction = new Restriction();
            foreach (var field in fieldNames)
            {
                restriction.Parameters.Add(field.AsParameter(), _entity.Fields[field]);
            }

            restriction.SetParameters(_command!);
            return builder.ToString();
        }

        private void ValidateInstanceCommand()
        {
            if (_command == null)
                throw new ArgumentNullException("The _command property can't be null.");
        }
    }
}
