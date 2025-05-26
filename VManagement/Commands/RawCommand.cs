using VManagement.Database.Clauses;

namespace VManagement.Database.Commands
{
    public class RawCommand
    {
        public string CommandText { get; set; } = string.Empty;
        public ParameterCollection Parameters { get; set; } = new ParameterCollection();

        public RawCommand() { }
        public RawCommand(string commandText)
        {
            CommandText = commandText;
        }
    }
}
