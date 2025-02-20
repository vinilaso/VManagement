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
        internal string UpdateClause => BuildUpdateClause();
        internal string DeleteClause => BuildDeleteClause();

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
            System.Diagnostics.Debugger.Launch();
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
            var validFieldNames = _entity.AllFieldNames(ignoreId: true)
                                         .Where(name => _entity.Fields[name] != null);

            builder.Append("INSERT INTO")
                   .Append(_entity.TableName)

                   .OpenParenthesis()
                   .AppendJoin(", ", validFieldNames)
                   .CloseParenthesis()

                   .Append("OUTPUT INSERTED.ID")
                   .Append("VALUES")

                   .OpenParenthesis()
                   .AppendJoin(", ", validFieldNames.Select(name => name.AsParameter()))
                   .CloseParenthesis();

            Restriction restriction = new Restriction();
            foreach (var field in validFieldNames)
            {
                restriction.Parameters.Add(field.AsParameter(), _entity.Fields[field]);
            }

            restriction.SetParameters(_command!);
            return builder.ToString();
        }

        private string BuildUpdateClause()
        {
            ValidateInstanceCommand();

            var builder = new DelimitedStringBuilder(" ");
            var validFieldNames = _entity.AllFieldNames(ignoreId: true);

            builder.Append("UPDATE")
                   .Append(_entity.TableName)
                   .Append("SET")
                   .AppendJoin(", ", validFieldNames.Select(name => $"{name} = {name.AsParameter()}"));

            
            if (Restriction != Restriction.Empty)
            {
                builder.Append(Restriction.ToString().Replace("A.", string.Empty));

                foreach (var field in validFieldNames)
                {
                    Restriction.Parameters.Add(field.AsParameter(), _entity.Fields[field]);
                }
            }

            Restriction.SetParameters(_command!);
            return builder.ToString();
        }

        private string BuildDeleteClause()
        {
            ValidateInstanceCommand();

            var builder = new DelimitedStringBuilder(" ");
            
            builder.Append("DELETE FROM")
                   .Append(_entity.TableName);

            if (Restriction == Restriction.Empty || Restriction == null)
                throw new OperationCanceledException("DELETE operations must have an restriction attached.");

            builder.Append(Restriction.ToString().Replace("A.", string.Empty));
            Restriction.Parameters.Add("@ID", _entity.Fields["ID"]);
            Restriction.SetParameters(_command!);

            return builder.ToString();
        }

        private void ValidateInstanceCommand()
        {
            if (_command == null)
                throw new ArgumentNullException("The _command property can't be null.");
        }
    }
}
