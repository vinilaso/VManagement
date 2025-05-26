
using VManagement.Commons.Entities.Attributes;
using VManagement.Core.Business;
using VManagement.Database;

Security.SetupEnvironment();

var novo = Teste.CreateInstance();

novo.Fields["NAME"] = "aLEX lIXO";
novo.Fields["DOTNETOBJECTNAME"] = "Lixão";
novo.Fields["NAMESPACE"] = "amooo";

novo.Save();

[EntityName("SQL_TABELAS")]
public class Teste : BusinessEntity<Teste>
{ 
}