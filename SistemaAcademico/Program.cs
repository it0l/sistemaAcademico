using Microsoft.EntityFrameworkCore;
using SistemaAcademico.Data;
using SistemaAcademico.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// ==================== ALUNOS ====================

app.MapGet("/alunos", (AppDbContext context) =>
{
    return context.Alunos.ToList();
});

app.MapGet("/alunos/{id}", (int id, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);
    if (aluno == null)
        return Results.NotFound("Aluno não encontrado");
    return Results.Ok(aluno);
});

app.MapPost("/alunos", (Aluno aluno, AppDbContext context) =>
{
    context.Alunos.Add(aluno);
    context.SaveChanges();
    return Results.Created($"/alunos/{aluno.Id}", aluno);
});

app.MapPut("/alunos/{id}", (int id, Aluno atualizado, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);
    if (aluno == null)
        return Results.NotFound("Aluno não encontrado");

    aluno.Nome = atualizado.Nome;
    aluno.Email = atualizado.Email;
    aluno.MatriculaNumero = atualizado.MatriculaNumero;
    aluno.DataNascimento = atualizado.DataNascimento;

    context.SaveChanges();
    return Results.Ok(aluno);
});

app.MapDelete("/alunos/{id}", (int id, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);
    if (aluno == null)
        return Results.NotFound("Aluno não encontrado");

    context.Alunos.Remove(aluno);
    context.SaveChanges();
    return Results.Ok("Aluno removido");
});

// ==================== CURSOS ====================

app.MapGet("/cursos", (AppDbContext context) =>
{
    return context.Cursos.ToList();
});

app.MapGet("/cursos/{id}", (int id, AppDbContext context) =>
{
    var curso = context.Cursos.Find(id);
    if (curso == null)
        return Results.NotFound("Curso não encontrado");
    return Results.Ok(curso);
});

app.MapPost("/cursos", (Curso curso, AppDbContext context) =>
{
    if (curso.CargaHoraria <= 0)
        return Results.BadRequest("Carga horária deve ser maior que zero");

    context.Cursos.Add(curso);
    context.SaveChanges();
    return Results.Created($"/cursos/{curso.Id}", curso);
});

app.MapPut("/cursos/{id}", (int id, Curso atualizado, AppDbContext context) =>
{
    if (atualizado.CargaHoraria <= 0)
        return Results.BadRequest("Carga horária deve ser maior que zero");

    var curso = context.Cursos.Find(id);
    if (curso == null)
        return Results.NotFound("Curso não encontrado");

    curso.Nome = atualizado.Nome;
    curso.Professor = atualizado.Professor;
    curso.CargaHoraria = atualizado.CargaHoraria;

    context.SaveChanges();
    return Results.Ok(curso);
});

app.MapDelete("/cursos/{id}", (int id, AppDbContext context) =>
{
    var curso = context.Cursos.Find(id);
    if (curso == null)
        return Results.NotFound("Curso não encontrado");

    context.Cursos.Remove(curso);
    context.SaveChanges();
    return Results.Ok("Curso removido");
});

// ==================== MATRÍCULAS ====================

app.MapGet("/matriculas", (AppDbContext context) =>
{
    return context.Matriculas
        .Include(m => m.Aluno)
        .Include(m => m.Curso)
        .ToList();
});

app.MapGet("/matriculas/{id}", (int id, AppDbContext context) =>
{
    var matricula = context.Matriculas
        .Include(m => m.Aluno)
        .Include(m => m.Curso)
        .FirstOrDefault(m => m.Id == id);

    if (matricula == null)
        return Results.NotFound("Matrícula não encontrada");
    return Results.Ok(matricula);
});

app.MapPost("/matriculas", (Matricula matricula, AppDbContext context) =>
{
    context.Matriculas.Add(matricula);
    context.SaveChanges();
    return Results.Created($"/matriculas/{matricula.Id}", matricula);
});

app.MapPut("/matriculas/{id}", (int id, Matricula atualizado, AppDbContext context) =>
{
    var matricula = context.Matriculas.Find(id);
    if (matricula == null)
        return Results.NotFound("Matrícula não encontrada");

    matricula.AlunoId = atualizado.AlunoId;
    matricula.CursoId = atualizado.CursoId;
    matricula.DataMatricula = atualizado.DataMatricula;
    matricula.Status = atualizado.Status;

    context.SaveChanges();
    return Results.Ok(matricula);
});

app.MapDelete("/matriculas/{id}", (int id, AppDbContext context) =>
{
    var matricula = context.Matriculas.Find(id);
    if (matricula == null)
        return Results.NotFound("Matrícula não encontrada");

    context.Matriculas.Remove(matricula);
    context.SaveChanges();
    return Results.Ok("Matrícula removida");
});

app.Run();
