namespace SistemaAcademico.Models;

public class Matricula
{
    public int Id { get; set; }

    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; } = null!;

    public int CursoId { get; set; }
    public Curso Curso { get; set; } = null!;

    public DateTime DataMatricula { get; set; }

    public string Status { get; set; } = string.Empty;
}
