using ClinicaMedica;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ConsultaRepository>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/catalogo", () => Results.Ok(new
{
    especialidades = Catalogo.Especialidades,
    medicos = Catalogo.MedicosPorEspecialidade,
    convenios = Catalogo.Convenios,
    exames = Catalogo.Exames
}));

app.MapGet("/api/consultas", async (string? busca, string? campo, string? status, string? especialidade, string? ordem, ConsultaRepository banco) =>
    Results.Ok(await banco.Listar(busca, campo, status, especialidade, ordem)));

app.MapPost("/api/consultas", async (Consulta consulta, ConsultaRepository banco) =>
{
    if (string.IsNullOrWhiteSpace(consulta.Paciente) || string.IsNullOrWhiteSpace(consulta.Medico))
        return Results.BadRequest(new { mensagem = "Informe o paciente e o médico." });
    if (!Catalogo.MedicosPorEspecialidade.ContainsKey(consulta.Especialidade))
        return Results.BadRequest(new { mensagem = "Selecione uma especialidade válida." });
    if (!Catalogo.MedicoPertenceAEspecialidade(consulta.Medico, consulta.Especialidade))
        return Results.BadRequest(new { mensagem = "Selecione um médico da especialidade escolhida." });
    if (Array.IndexOf(Catalogo.Convenios, consulta.Convenio) < 0)
        return Results.BadRequest(new { mensagem = "Selecione um convênio válido." });
    if (!string.IsNullOrEmpty(consulta.Exame) && Array.IndexOf(Catalogo.Exames, consulta.Exame) < 0)
        return Results.BadRequest(new { mensagem = "Selecione um exame válido." });
    if (consulta.DataHora < DateTime.Now)
        return Results.BadRequest(new { mensagem = "A consulta deve ter uma data futura." });
    if (consulta.Status is not ("Agendada" or "Concluída" or "Cancelada"))
        return Results.BadRequest(new { mensagem = "Selecione uma situação válida." });

    var id = await banco.Adicionar(consulta);
    return Results.Created($"/api/consultas/{id}", new { id });
});

app.MapDelete("/api/consultas/{id:int}", async (int id, ConsultaRepository banco) =>
    await banco.Remover(id) ? Results.NoContent() : Results.NotFound());

app.Run();
