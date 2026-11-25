using Microsoft.Data.Sqlite;

namespace ClinicaMedica;

public class ConsultaRepository
{
    private readonly string _conexao;

    public ConsultaRepository(IWebHostEnvironment ambiente)
    {
        var pasta = Path.Combine(ambiente.ContentRootPath, "dados");
        Directory.CreateDirectory(pasta);
        _conexao = $"Data Source={Path.Combine(pasta, "clinica.db")}";

        using var banco = new SqliteConnection(_conexao);
        banco.Open();
        using var comando = banco.CreateCommand();
        comando.CommandText = """
            CREATE TABLE IF NOT EXISTS Consultas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Paciente TEXT NOT NULL,
                Medico TEXT NOT NULL,
                Especialidade TEXT NOT NULL,
                DataHora TEXT NOT NULL,
                Status TEXT NOT NULL
            );
            """;
        comando.ExecuteNonQuery();

        var colunas = new List<string>();
        comando.CommandText = "PRAGMA table_info(Consultas)";
        using (var leitor = comando.ExecuteReader())
            while (leitor.Read()) colunas.Add(leitor.GetString(1));
        foreach (var coluna in new[] { "Convenio", "Exame" })
        {
            if (colunas.Contains(coluna)) continue;
            comando.CommandText = $"ALTER TABLE Consultas ADD COLUMN {coluna} TEXT NOT NULL DEFAULT ''";
            comando.ExecuteNonQuery();
        }
    }

    public async Task<List<Consulta>> Listar(string? busca, string? campo, string? status, string? especialidade, string? ordem)
    {
        await using var banco = new SqliteConnection(_conexao);
        await banco.OpenAsync();
        await using var comando = banco.CreateCommand();
        comando.CommandText = "SELECT Id, Paciente, Medico, Especialidade, DataHora, Status, Convenio, Exame FROM Consultas";

        var consultas = new List<Consulta>();
        await using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            consultas.Add(new Consulta
            {
                Id = leitor.GetInt32(0), Paciente = leitor.GetString(1), Medico = leitor.GetString(2),
                Especialidade = leitor.GetString(3), DataHora = DateTime.Parse(leitor.GetString(4)), Status = leitor.GetString(5),
                Convenio = leitor.GetString(6) == "" ? "PARTICULAR" : leitor.GetString(6), Exame = leitor.GetString(7)
            });
        }

        var resultado = BuscaLinear.Buscar(consultas, busca, campo);
        resultado = Selecao.Selecionar(resultado, status, especialidade);
        SelectionSort.Ordenar(resultado,
            ordem == "paciente" ? SelectionSort.PorPaciente : SelectionSort.PorData);

        return resultado;
    }

    public async Task<int> Adicionar(Consulta consulta)
    {
        await using var banco = new SqliteConnection(_conexao);
        await banco.OpenAsync();
        await using var comando = banco.CreateCommand();
        comando.CommandText = "INSERT INTO Consultas (Paciente, Medico, Especialidade, DataHora, Status, Convenio, Exame) VALUES ($paciente, $medico, $especialidade, $data, $status, $convenio, $exame); SELECT last_insert_rowid();";
        comando.Parameters.AddWithValue("$paciente", consulta.Paciente.Trim());
        comando.Parameters.AddWithValue("$medico", consulta.Medico.Trim());
        comando.Parameters.AddWithValue("$especialidade", consulta.Especialidade.Trim());
        comando.Parameters.AddWithValue("$data", consulta.DataHora.ToString("O"));
        comando.Parameters.AddWithValue("$status", consulta.Status);
        comando.Parameters.AddWithValue("$convenio", consulta.Convenio);
        comando.Parameters.AddWithValue("$exame", consulta.Exame ?? "");
        return Convert.ToInt32(await comando.ExecuteScalarAsync());
    }

    public async Task<bool> Remover(int id)
    {
        await using var banco = new SqliteConnection(_conexao);
        await banco.OpenAsync();
        await using var comando = banco.CreateCommand();
        comando.CommandText = "DELETE FROM Consultas WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        return await comando.ExecuteNonQueryAsync() > 0;
    }
}
