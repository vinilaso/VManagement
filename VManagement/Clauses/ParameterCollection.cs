using Microsoft.Data.SqlClient;

namespace VManagement.Database.Clauses
{
    public class ParameterCollection : List<SqlParameter>
    {
        public object? this[string parameterName]
        {
            get => ByName(parameterName).Value;
            set => ByName(parameterName).Value = value;
        }

        public void Add(string parameterName, object? parameterValue)
        {
            SqlParameter parameter = new(parameterName, parameterValue ?? DBNull.Value);

            if (parameterValue is string)
            {
                parameter.CompareInfo = System.Data.SqlTypes.SqlCompareOptions.None;
            }

            Add(parameter);
        }

        public void Add(string parameterName, bool parameterValue)
        {
            if (parameterValue)
            {
                Add(parameterName, "Y");
            }
            else
            {
                Add(parameterName, "N");
            }
        }

        private SqlParameter ByName(string parameterName)
        {
            var parameter = Find(param => param.ParameterName == parameterName);

            if (parameter == null)
                throw new IndexOutOfRangeException($"There is no parameter named {parameterName} in this collection.");

            return parameter;
        }
    }
}
