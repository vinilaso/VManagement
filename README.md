# VManagement

Biblioteca de gerenciamento de entidades.
Utilizada para estudos pessoais.

# Instruções de uso

Crie uma classe que implementa Entity e IEntity e siga as instruções abaixo:

```csharp
public class Program
{
    static void Main()
    {
        Pessoas pessoa = new Pessoas();
        pessoa.Nome = "Maria da Silva";
        pessoa.DataNascimento = new DateTime(2006, 4, 12);
        pessoa.Cpf = string.Empty;

        pessoa.Save(); // Pronto! Seu objeto foi salvo no banco de dados
    }
}

public class Pessoas : Entity, IEnity
{
   public override long Id { get; set; } // Chave primária da entidade
   public override string Name { get => "Pessoas"; } // Nome da entidade no banco de dados
   public override Dictionary<string, object?> Fields { get; } = new Dictionary<string, object?>() // Inicie seu KeyValuePair com os campos da sua entidade
              {
                  {"NOME", null},
                  {"DATANASCIMENTO", null},
                  {"CPF", null}
              };

    // Se preferir, adicione propriedades que referenciem seu KeyValuePair.
    public string Nome
    {
        get
        {
            try
            {
                return Fields["NOME"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        set
        {
            Fields["NOME"] = value;
        }
    }

    public DateTime DataNascimento
    {
        get
        {
            try
            {
                return (DateTime) Fields["DATANASCIMENTO"];
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        set
        {
            Fields["DATANASCIMENTO"] = value;
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
```
