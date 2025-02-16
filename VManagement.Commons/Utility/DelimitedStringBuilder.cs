using System.Text;

namespace VManagement.Commons.Utility
{
    public class DelimitedStringBuilder
    {
        private StringBuilder _builder = new StringBuilder();
        private string _delimiter;
        private Dictionary<char, char> _parenthesisPairs = new Dictionary<char, char>();

        public string Delimiter => _delimiter;

        public DelimitedStringBuilder(string delimiter)
        {
            _delimiter = delimiter;
        }

        public DelimitedStringBuilder Append(string value)
        {
            if (_builder.Length > 0)
            {
                _builder.Append(_delimiter);
            }

            _builder.Append(value);
            return this;
        }

        public DelimitedStringBuilder AppendRaw(string value)
        {
            _builder.Append(value);
            return this;
        }

        public DelimitedStringBuilder AppendJoin(string separator, IEnumerable<string> values)
        {
            this.Append(string.Join(separator, values));
            return this;
        }

        public DelimitedStringBuilder AppendJoinRaw(string separator, params string[] values)
        {
            this.AppendRaw(string.Join(separator, values));
            return this;
        }

        public DelimitedStringBuilder OpenParenthesis(bool raw = false)
        {
            if (raw)
            {
                return AppendRaw("(");
            }

            return Append("(");
        }

        public DelimitedStringBuilder CloseParenthesis(bool raw = false)
        {

            if (raw)
            {
                return AppendRaw(")");
            }

            return Append(")");
        }

        public DelimitedStringBuilder Clear()
        {
            _builder.Clear();
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
