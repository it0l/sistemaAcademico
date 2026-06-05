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

// ==================== ALUNOS ====================

app.MapGet("/alunos", (AppDbContext context) =>
{
    var alunos = context.Alunos
        .Include(a => a.Matriculas)
        .ThenInclude(m => m.Curso)
        .Select(a => new AlunoResponseDto
        {
            Id = a.Id,
            Nome = a.Nome,
            Email = a.Email,
            MatriculaNumero = a.MatriculaNumero,
            DataNascimento = a.DataNascimento,

            Matriculas = a.Matriculas.Select(m => new MatriculaResumidaDto
            {
                Id = m.Id,
                DataMatricula = m.DataMatricula,
                Status = m.Status,

                Curso = m.Curso == null ? null : new CursoResumidoDto
                {
                    Id = m.Curso.Id,
                    Nome = m.Curso.Nome,
                    Professor = m.Curso.Professor,
                    CargaHoraria = m.Curso.CargaHoraria
                }
            }).ToList()
        })
        .ToList();

    return Results.Ok(alunos);
});

app.MapGet("/alunos/{id}", (int id, AppDbContext context) =>
{
    var aluno = context.Alunos
        .Include(a => a.Matriculas)
        .ThenInclude(m => m.Curso)
        .Where(a => a.Id == id)
        .Select(a => new AlunoResponseDto
        {
            Id = a.Id,
            Nome = a.Nome,
            Email = a.Email,
            MatriculaNumero = a.MatriculaNumero,
            DataNascimento = a.DataNascimento,

            Matriculas = a.Matriculas.Select(m => new MatriculaResumidaDto
            {
                Id = m.Id,
                DataMatricula = m.DataMatricula,
                Status = m.Status,

                Curso = m.Curso == null ? null : new CursoResumidoDto
                {
                    Id = m.Curso.Id,
                    Nome = m.Curso.Nome,
                    Professor = m.Curso.Professor,
                    CargaHoraria = m.Curso.CargaHoraria
                }
            }).ToList()
        })
        .FirstOrDefault();

    if (aluno == null)
        return Results.NotFound("Aluno não encontrado");

    return Results.Ok(aluno);
});

app.MapPost("/alunos", (AlunoDto dto, AppDbContext context) =>
{
    var aluno = new Aluno
    {
        Nome = dto.Nome,
        Email = dto.Email,
        MatriculaNumero = dto.MatriculaNumero,
        DataNascimento = dto.DataNascimento
    };

    context.Alunos.Add(aluno);
    context.SaveChanges();

    return Results.Created($"/alunos/{aluno.Id}", aluno);
});

app.MapPut("/alunos/{id}", (int id, AlunoDto dto, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(id);

    if (aluno == null)
        return Results.NotFound("Aluno não encontrado");

    aluno.Nome = dto.Nome;
    aluno.Email = dto.Email;
    aluno.MatriculaNumero = dto.MatriculaNumero;
    aluno.DataNascimento = dto.DataNascimento;

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

app.MapPost("/cursos", (CursoDto dto, AppDbContext context) =>
{
    if (dto.CargaHoraria <= 0)
        return Results.BadRequest("Carga horária deve ser maior que zero");

    var curso = new Curso
    {
        Nome = dto.Nome,
        Professor = dto.Professor,
        CargaHoraria = dto.CargaHoraria
    };

    context.Cursos.Add(curso);
    context.SaveChanges();

    return Results.Created($"/cursos/{curso.Id}", curso);
});

app.MapPut("/cursos/{id}", (int id, CursoDto dto, AppDbContext context) =>
{
    var curso = context.Cursos.Find(id);

    if (curso == null)
        return Results.NotFound("Curso não encontrado");

    if (dto.CargaHoraria <= 0)
        return Results.BadRequest("Carga horária deve ser maior que zero");

    curso.Nome = dto.Nome;
    curso.Professor = dto.Professor;
    curso.CargaHoraria = dto.CargaHoraria;

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
    var matriculas = context.Matriculas
        .Include(m => m.Aluno)
        .Include(m => m.Curso)
        .Select(m => new MatriculaResponseDto
        {
            Id = m.Id,
            NomeAluno = m.Aluno!.Nome,
            NomeCurso = m.Curso!.Nome,
            DataMatricula = m.DataMatricula,
            Status = m.Status
        })
        .ToList();

    return Results.Ok(matriculas);
});

app.MapGet("/matriculas/{id}", (int id, AppDbContext context) =>
{
    var matricula = context.Matriculas
        .Include(m => m.Aluno)
        .Include(m => m.Curso)
        .Where(m => m.Id == id)
        .Select(m => new MatriculaResponseDto
        {
            Id = m.Id,
            NomeAluno = m.Aluno!.Nome,
            NomeCurso = m.Curso!.Nome,
            DataMatricula = m.DataMatricula,
            Status = m.Status
        })
        .FirstOrDefault();

    if (matricula == null)
        return Results.NotFound();

    return Results.Ok(matricula);
});

app.MapPost("/matriculas", (MatriculaDto dto, AppDbContext context) =>
{
    var aluno = context.Alunos.Find(dto.AlunoId);

    if (aluno == null)
        return Results.BadRequest("Aluno não encontrado");

    var curso = context.Cursos.Find(dto.CursoId);

    if (curso == null)
        return Results.BadRequest("Curso não encontrado");

    var matricula = new Matricula
    {
        AlunoId = dto.AlunoId,
        CursoId = dto.CursoId,
        DataMatricula = dto.DataMatricula,
        Status = dto.Status
    };

    context.Matriculas.Add(matricula);
    context.SaveChanges();

    return Results.Created($"/matriculas/{matricula.Id}", matricula);
});

app.MapPut("/matriculas/{id}", (int id, MatriculaDto dto, AppDbContext context) =>
{
    var matricula = context.Matriculas.Find(id);

    if (matricula == null)
        return Results.NotFound();

    matricula.AlunoId = dto.AlunoId;
    matricula.CursoId = dto.CursoId;
    matricula.DataMatricula = dto.DataMatricula;
    matricula.Status = dto.Status;

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
