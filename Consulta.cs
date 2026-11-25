namespace ClinicaMedica;

public class Consulta
{
    public int Id { get; set; }
    public string Paciente { get; set; } = "";
    public string Medico { get; set; } = "";
    public string Especialidade { get; set; } = "";
    public string Convenio { get; set; } = "PARTICULAR";
    public string Exame { get; set; } = "";
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "Agendada";
}
