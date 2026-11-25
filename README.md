# Clínica Médica

Projeto acadêmico simples para cadastrar e organizar consultas médicas. Feito com C#, ASP.NET Core, HTML, CSS, JavaScript e SQLite.

## O que dá para fazer

- Cadastrar uma consulta com paciente, médico, especialidade, data e situação.
- **Busca** (busca linear manual): por texto + campo (paciente, médico, especialidade ou todos).
- **Seleção** (filtragem manual): por situação da consulta + especialidade.
- **Ordenação** (Selection Sort manual): por data ou por nome do paciente.
- Excluir uma consulta.

## Como executar

1. Instale o SDK do .NET 8: https://dotnet.microsoft.com/download/dotnet/8.0
2. Abra o terminal nesta pasta.
3. Execute `dotnet restore`.
4. Execute `dotnet run`.
5. Abra o endereço local mostrado no terminal (normalmente `http://localhost:5000`).
6. Para terminar de executar use Ctrl+C

O banco de dados `dados/clinica.db` é criado automaticamente na primeira execução.

## Algoritmos e estrutura

Todos os algoritmos são escritos à mão (sem LINQ, `OrderBy`, `Where`, `LIKE` ou `ORDER BY`) e operam sobre uma estrutura de dados linear: `List<Consulta>`. O SQLite é usado apenas para armazenar e ler os registros.

| Algoritmo | Arquivo | Parâmetros |
|---|---|---|
| Busca linear | `BuscaLinear.cs` | texto, campo |
| Seleção | `Selecao.cs` | status, especialidade |
| Selection Sort | `SelectionSort.cs` | critério: paciente ou data |

## Organização

- `Program.cs`: inicialização da aplicação e rotas da API.
- `Consulta.cs`: dados de uma consulta.
- `ConsultaRepository.cs`: comandos para guardar e ler dados no SQLite; carrega todos os registros em uma `List<Consulta>` e aplica, em ordem, busca linear, seleção e Selection Sort.
- `Catalogo.cs`: listas de especialidades, médicos, convênios e exames usadas nas caixas de seleção.
- `BuscaLinear.cs`: busca sequencial manual com dois parâmetros (texto e campo).
- `Selecao.cs`: seleção manual com dois parâmetros (status e especialidade).
- `SelectionSort.cs`: implementação manual do Selection Sort com critérios por nome do paciente e por data/hora.
- `wwwroot/`: página, estilos e interações do navegador.

> Este projeto é um exercício acadêmico. Todos os dados de pacientes usados é fictício.
