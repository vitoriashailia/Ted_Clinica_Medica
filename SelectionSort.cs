using System.Globalization;

namespace ClinicaMedica;
public static class SelectionSort
{
    public static void Ordenar(List<Consulta> lista, Comparison<Consulta> comparar)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            int posicaoMenor = i;
            for (int j = i + 1; j < lista.Count; j++)
            {
                if (comparar(lista[j], lista[posicaoMenor]) < 0)
                    posicaoMenor = j;
            }

            if (posicaoMenor != i)
                (lista[i], lista[posicaoMenor]) = (lista[posicaoMenor], lista[i]);
        }
    }

    public static int PorPaciente(Consulta a, Consulta b)
    {
        int resultado = string.Compare(a.Paciente, b.Paciente, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
        if (resultado != 0) return resultado;
        resultado = a.DataHora.CompareTo(b.DataHora);
        return resultado != 0 ? resultado : a.Id.CompareTo(b.Id);
    }

    public static int PorData(Consulta a, Consulta b)
    {
        int resultado = a.DataHora.CompareTo(b.DataHora);
        if (resultado != 0) return resultado;
        resultado = string.Compare(a.Paciente, b.Paciente, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
        return resultado != 0 ? resultado : a.Id.CompareTo(b.Id);
    }
}
