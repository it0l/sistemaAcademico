using Microsoft.EntityFrameworkCore;
using SistemaAcademico.Data;
using SistemaAcademico.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=sistema.db"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Sistema Acadêmico API");


// LISTAR ALUNOS
app.MapGet("/alunos", async (AppDbContext db) =>
{
    return await db.Alunos.ToListAsync();
});


// BUSCAR ALUNO POR ID
app.MapGet("/alunos/{id}", async (int id, AppDbContext db) =>
{
    var aluno = await db.Alunos.FindAsync(id);

    return aluno is null
        ? Results.NotFound("Aluno não encontrado")
        : Results.Ok(aluno);
});


// CADASTRAR ALUNO
app.MapPost("/alunos", async (Aluno aluno, AppDbContext db) =>
{
    db.Alunos.Add(aluno);

    await db.SaveChangesAsync();

    return Results.Created($"/alunos/{aluno.Id}", aluno);
});


// ATUALIZAR ALUNO
app.MapPut("/alunos/{id}", async (int id, Aluno dados, AppDbContext db) =>
{
    var aluno = await db.Alunos.FindAsync(id);

    if (aluno is null)
        return Results.NotFound("Aluno não encontrado");

    aluno.Nome = dados.Nome;
    aluno.Email = dados.Email;
    aluno.MatriculaNumero = dados.MatriculaNumero;
    aluno.DataNascimento = dados.DataNascimento;

    await db.SaveChangesAsync();

    return Results.Ok(aluno);
});


// REMOVER ALUNO
app.MapDelete("/alunos/{id}", async (int id, AppDbContext db) =>
{
    var aluno = await db.Alunos.FindAsync(id);

    if (aluno is null)
        return Results.NotFound("Aluno não encontrado");

    db.Alunos.Remove(aluno);

    await db.SaveChangesAsync();

    return Results.Ok("Aluno removido com sucesso");
});

app.Run();