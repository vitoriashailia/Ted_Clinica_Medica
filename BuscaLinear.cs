namespace ClinicaMedica;

public static class BuscaLinear
{
    public static List<Consulta> Buscar(List<Consulta> lista, string? texto, string? campo)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return new List<Consulta>(lista);

        var termo = texto.Trim();
        var encontrados = new List<Consulta>();

        for (int i = 0; i < lista.Count; i++)
        {
            var consulta = lista[i];
            bool achou = campo switch
            {
                "paciente" => Contem(consulta.Paciente, termo),
                "medico" => Contem(consulta.Medico, termo),
                "especialidade" => Contem(consulta.Especialidade, termo),
                _ => Contem(consulta.Paciente, termo) || Contem(consulta.Medico, termo) || Contem(consulta.Especialidade, termo)
            };

            if (achou)
                encontrados.Add(consulta);
        }

        return encontrados;
    }

    private static bool Contem(string valor, string termo) =>
        valor.Contains(termo, StringComparison.OrdinalIgnoreCase);
}
