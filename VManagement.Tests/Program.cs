using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using VManagement.Core.Entities;
using VManagement.Database;
using Microsoft.Extensions.Configuration;
using VManagement.Commons.Interfaces;

Pessoas pessoa = new Pessoas();

var pessoas = pessoa.GetAll("NOME = 'Jose'", string.Empty);

pessoa.Nome = "Alex";
pessoa.Cpf = "1234";
pessoa.DataNascimento = new DateTime(2006, 04, 12);

pessoa.Save();

public class Pessoas : Entity, IEntity
{
    public override long Id { get => base.Id; set => base.Id = value; }
    public override string Name { get => "Pessoas"; }
    public override Dictionary<string, object?> Fields { get; } = new Dictionary<string, object?>()
            {
                { "Nome", null },
                { "DataNascimento", null },
                { "CPF", null }
            };

    public string Nome
    {
        get
        {
            try
            {
                return Fields["Nome"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        set
        {
            Fields["Nome"] = value;
        }
    }
    public DateTime DataNascimento
    {
        get
        {
            try
            {
                return (DateTime) Fields["DataNascimento"];
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        set
        {
            Fields["DataNascimento"] = value;
        }
    }
    public string Cpf
    {
        get
        {
            try
            {
                return Fields["CPF"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        set
        {
            Fields["CPF"] = value;
        }
    }
}
