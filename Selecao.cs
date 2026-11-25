namespace ClinicaMedica;

public static class Selecao
{
    public static List<Consulta> Selecionar(List<Consulta> lista, string? status, string? especialidade)
    {
        bool filtraStatus = !string.IsNullOrWhiteSpace(status) && status != "Todas";
        bool filtraEspecialidade = !string.IsNullOrWhiteSpace(especialidade);
        var alvoEspecialidade = especialidade?.Trim() ?? "";

        var selecionadas = new List<Consulta>();

        for (int i = 0; i < lista.Count; i++)
        {
            var consulta = lista[i];

            if (filtraStatus && consulta.Status != status)
                continue;

            if (filtraEspecialidade &&
                !string.Equals(consulta.Especialidade.Trim(), alvoEspecialidade, StringComparison.OrdinalIgnoreCase))
                continue;

            selecionadas.Add(consulta);
        }

        return selecionadas;
    }
}
