namespace ClinicaMedica;

public static class Catalogo
{
    public static readonly Dictionary<string, string[]> MedicosPorEspecialidade = new()
    {
        ["Oncologia"] = new[] { "Dra. Helena Duarte - CRM 1001", "Dr. Rafael Tavares - CRM 1002" },
        ["Pediatria"] = new[] { "Dra. Juliana Mendes - CRM 2001", "Dr. Ricardo Paiva - CRM 2002" },
        ["Neurologia"] = new[] { "Dr. Eduardo Ramos - CRM 3001", "Dra. Cláudia Fonseca - CRM 3002" },
        ["Clínico geral"] = new[] { "Dr. Carlos Almeida - CRM 4001", "Dra. Beatriz Nogueira - CRM 4002" },
        ["Cardiologista"] = new[] { "Dr. João Lima - CRM 5001", "Dra. Marina Castro - CRM 5002" }
    };

    public static readonly string[] Convenios =
    {
        "PARTICULAR", "Saúde Vida", "MedPlus", "Bem Estar Saúde", "UniClin", "Vida Nova Saúde"
    };

    public static readonly string[] Exames =
    {
        "Raio x", "Ressonância magnética", "Ultrassonografia", "Hemograma", "PCR", "Lactato venoso",
        "EAS", "Hemocultura", "Coprocultura", "Urocultura", "LBA", "Bilirrubinas total",
        "Bilirrubinas fracionadas", "Coombs", "Tipagem sanguínea", "Vitaminas", "Cálcio", "Potássio", "Eletrólitos"
    };

    public static List<string> Especialidades => new(MedicosPorEspecialidade.Keys);

    public static bool MedicoPertenceAEspecialidade(string medico, string especialidade) =>
        MedicosPorEspecialidade.TryGetValue(especialidade, out var medicos) && Array.IndexOf(medicos, medico) >= 0;
}
