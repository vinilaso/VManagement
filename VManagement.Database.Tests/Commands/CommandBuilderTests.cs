using System.Text.RegularExpressions;
using VManagement.Commons.Entities;
using VManagement.Database.Clauses;
using VManagement.Database.Commands;

namespace VManagement.Database.Tests.Commands
{
    [TestClass]
    public partial class CommandBuilderTests
    {
        [TestMethod]
        public void Constroi_Select_DeveRetornarCorreto()
        {
            const string resultadoEsperado = "SELECT A.ID,A.NOME,A.DATANASCIMENTO FROM GEN_PESSOAS A WHERE (A.ID = 999) AND (A.DATANASCIMENTO > 999)";

            var entidade = new CoreEntity();

            Restriction restriction = new Restriction("A.ID = 999");
            restriction.AddWhereClause("A.DATANASCIMENTO > 999");

            CommandBuilder commandBuilder = new CommandBuilder(entidade, restriction);

            string resultadoEsperadoSemEspacos = MyRegex().Replace(resultadoEsperado, " ").Trim();
            string resultadoAtualSemEspacos = MyRegex().Replace(commandBuilder.BuildSelectClause(), " ").Trim();

            Assert.AreEqual(resultadoEsperadoSemEspacos, resultadoAtualSemEspacos);
        }

        [TestMethod]
        public void Constroi_Update_DeveRetornarCorreto()
        {
            const string resultadoEsperado = "UPDATE GEN_PESSOAS SET NOME = @NOME,DATANASCIMENTO = @DATANASCIMENTO WHERE (ID = 999)";

            var entidade = new CoreEntity();


            Restriction restriction = new Restriction("ID = 999");
            CommandBuilder commandBuilder = new CommandBuilder(entidade, restriction);
            string resultadoEsperadoSemEspacos = MyRegex().Replace(resultadoEsperado, " ").Trim();
            string resultadoAtualSemEspacos = MyRegex().Replace(commandBuilder.BuildUpdateClause(), " ").Trim();

            Assert.AreEqual(resultadoEsperadoSemEspacos, resultadoAtualSemEspacos);
        }

        [TestMethod]
        public void Constroi_Insert_DeveRetornarCorreto()
        {
            const string resultadoEsperado = "INSERT INTO GEN_PESSOAS (NOME,DATANASCIMENTO) VALUES (@NOME,@DATANASCIMENTO)";

            var entidade = new CoreEntity();


            CommandBuilder commandBuilder = new CommandBuilder(entidade);
            string resultadoEsperadoSemEspacos = MyRegex().Replace(resultadoEsperado, " ").Trim();
            string resultadoAtualSemEspacos = MyRegex().Replace(commandBuilder.BuildInsertClause(), " ").Trim();

            Assert.AreEqual(resultadoEsperadoSemEspacos, resultadoAtualSemEspacos);
        }

        [TestMethod]
        public void Constroi_Delete_DeveRetornarCorreto()
        {
            const string resultadoEsperado = "DELETE FROM GEN_PESSOAS A WHERE (A.ID = 999)";

            var entidade = new CoreEntity();


            Restriction restriction = new Restriction("A.ID = 999");
            CommandBuilder commandBuilder = new CommandBuilder(entidade, restriction);
            string resultadoEsperadoSemEspacos = MyRegex().Replace(resultadoEsperado, " ").Trim();
            string resultadoAtualSemEspacos = MyRegex().Replace(commandBuilder.BuildDeleteClause(), " ").Trim();

            Assert.AreEqual(resultadoEsperadoSemEspacos, resultadoAtualSemEspacos);
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex MyRegex();
    }
}
