using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SistemaAcademico.Data;
using SistemaAcademico.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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

app.MapGet("/alunos", (AppDbContext context) =>
{
    var listaAlunos = context.Alunos.ToList();
    var listaMatriculas = context.Matriculas.ToList();

    foreach (var aluno in listaAlunos)
    {
        aluno.Matriculas.Clear(); 
        
        foreach (var m in listaMatriculas)
        {
            if (m.AlunoId == aluno.Id)
            {
                m.Curso = context.Cursos.Find(m.CursoId);
                aluno.Matriculas.Add(m);
            }
        }
    }

    return Results.Ok(listaAlunos);
});

app.MapGet("/alunos/{id}", (int id, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);

    if (aluno == null)
        return Results.NotFound("Aluno nao encontrado");

    var listaMatriculas = context.Matriculas.ToList();
    
    aluno.Matriculas.Clear();
    
    foreach (var m in listaMatriculas)
    {
        if (m.AlunoId == aluno.Id)
        {
            m.Curso = context.Cursos.Find(m.CursoId);
            aluno.Matriculas.Add(m);
        }
    }

    return Results.Ok(aluno);
});
app.MapPost("/alunos", (Aluno aluno, AppDbContext context) =>
{
    if (string.IsNullOrWhiteSpace(aluno.Nome))
        return Results.BadRequest("Nome do aluno e obrigatorio");

    if (string.IsNullOrWhiteSpace(aluno.Email))
        return Results.BadRequest("Email do aluno e obrigatorio");

    context.Alunos.Add(aluno);
    context.SaveChanges();

    return Results.Created($"/alunos/{aluno.Id}", aluno);
});

app.MapPut("/alunos/{id}", (int id, Aluno alunoAlterado, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);

    if (aluno == null)
        return Results.NotFound("Aluno nao encontrado");

    if (string.IsNullOrWhiteSpace(alunoAlterado.Nome))
        return Results.BadRequest("Nome do aluno e obrigatorio");

    if (string.IsNullOrWhiteSpace(alunoAlterado.Email))
        return Results.BadRequest("Email do aluno e obrigatorio");

    aluno.Nome = alunoAlterado.Nome;
    aluno.Email = alunoAlterado.Email;
    aluno.MatriculaNumero = alunoAlterado.MatriculaNumero;
    aluno.DataNascimento = alunoAlterado.DataNascimento;

    context.SaveChanges();
    return Results.Ok(aluno);
});

app.MapDelete("/alunos/{id}", (int id, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);
    if (aluno == null)
        return Results.NotFound("Aluno nao encontrado");

    context.Alunos.Remove(aluno);
    context.SaveChanges();
    return Results.Ok("Aluno removido");
});

app.MapGet("/cursos", (AppDbContext context) =>
{
    return Results.Ok(context.Cursos);
});

app.MapGet("/cursos/{id}", (int id, AppDbContext context) =>
{
    var curso = context.Cursos.Find(id);
    if (curso == null)
        return Results.NotFound("Curso nao encontrado");
        
    return Results.Ok(curso);
});

app.MapPost("/cursos", (Curso curso, AppDbContext context) =>
{
    if (string.IsNullOrWhiteSpace(curso.Nome))
        return Results.BadRequest("Nome do curso e obrigatorio");

    if (string.IsNullOrWhiteSpace(curso.Professor))
        return Results.BadRequest("Nome do professor e obrigatorio");

    if (curso.CargaHoraria <= 0)
        return Results.BadRequest("Carga horaria deve ser maior que zero");

    context.Cursos.Add(curso);
    context.SaveChanges();

    return Results.Created($"/cursos/{curso.Id}", curso);
});

app.MapPut("/cursos/{id}", (int id, Curso cursoAlterado, AppDbContext context) =>
{
    var curso = context.Cursos.Find(id);

    if (curso == null)
        return Results.NotFound("Curso nao encontrado");

    if (string.IsNullOrWhiteSpace(cursoAlterado.Nome))
        return Results.BadRequest("Nome do curso e obrigatorio");

    if (string.IsNullOrWhiteSpace(cursoAlterado.Professor))
        return Results.BadRequest("Nome do professor e obrigatorio");

    if (cursoAlterado.CargaHoraria <= 0)
        return Results.BadRequest("Carga horaria deve ser maior que zero");

    curso.Nome = cursoAlterado.Nome;
    curso.Professor = cursoAlterado.Professor;
    curso.CargaHoraria = cursoAlterado.CargaHoraria;

    context.SaveChanges();
    return Results.Ok(curso);
});

app.MapDelete("/cursos/{id}", (int id, AppDbContext context) =>
{
    var curso = context.Cursos.Find(id);
    if (curso == null)
        return Results.NotFound("Curso nao encontrado");

    context.Cursos.Remove(curso);
    context.SaveChanges();
    return Results.Ok("Curso removido");
});

app.MapGet("/matriculas", (AppDbContext context) =>
{
    var listaMatriculas = context.Matriculas.ToList();

    foreach (var m in listaMatriculas)
    {
        m.Curso = context.Cursos.Find(m.CursoId);
    }

    return Results.Ok(listaMatriculas);
});

app.MapGet("/matriculas/{id}", (int id, AppDbContext context) =>
{
    var matricula = context.Matriculas.Find(id);

    if (matricula == null)
        return Results.NotFound("Matricula nao encontrada");

    matricula.Curso = context.Cursos.Find(matricula.CursoId);

    return Results.Ok(matricula);
});

app.MapPost("/matriculas", (Matricula matricula, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(matricula.AlunoId);
    if (aluno == null)
        return Results.BadRequest("Aluno nao encontrado");

    var curso = context.Cursos.Find(matricula.CursoId);
    if (curso == null)
        return Results.BadRequest("Curso nao encontrado");

    bool duplicada = false;
    foreach (var m in context.Matriculas)
    {
        if (m.AlunoId == matricula.AlunoId && m.CursoId == matricula.CursoId)
        {
            duplicada = true;
            break;
        }
    }

    if (duplicada)
        return Results.BadRequest("Aluno ja esta matriculado neste curso");

    context.Matriculas.Add(matricula);
    context.SaveChanges();

    return Results.Created($"/matriculas/{matricula.Id}", matricula);
});

app.MapPut("/matriculas/{id}", (int id, Matricula matriculaAlterada, AppDbContext context) =>
{
    var matricula = context.Matriculas.Find(id);

    if (matricula == null)
        return Results.NotFound("Matricula nao encontrada");

    var aluno = context.Alunos.Find(matriculaAlterada.AlunoId);
    if (aluno == null)
        return Results.BadRequest("Aluno nao encontrado");

    var curso = context.Cursos.Find(matriculaAlterada.CursoId);
    if (curso == null)
        return Results.BadRequest("Curso nao encontrado");

    if (matricula.AlunoId != matriculaAlterada.AlunoId || matricula.CursoId != matriculaAlterada.CursoId)
    {
        bool duplicada = false;
        foreach (var m in context.Matriculas)
        {
            if (m.Id != id && m.AlunoId == matriculaAlterada.AlunoId && m.CursoId == matriculaAlterada.CursoId)
            {
                duplicada = true;
                break;
            }
        }

        if (duplicada)
            return Results.BadRequest("Aluno ja esta matriculado neste curso");
    }

    matricula.AlunoId = matriculaAlterada.AlunoId;
    matricula.CursoId = matriculaAlterada.CursoId;
    matricula.DataMatricula = matriculaAlterada.DataMatricula;
    matricula.Status = matriculaAlterada.Status;

    context.SaveChanges();
    return Results.Ok(matricula);
});

app.MapDelete("/matriculas/{id}", (int id, AppDbContext context) =>
{
    var matricula = context.Matriculas.Find(id);
    if (matricula == null)
        return Results.NotFound("Matricula nao encontrada");

    context.Matriculas.Remove(matricula);
    context.SaveChanges();
    return Results.Ok("Matricula removida");
});

app.Run();