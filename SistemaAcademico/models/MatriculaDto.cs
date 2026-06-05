namespace SistemaAcademico.Models;

public class MatriculaDto
{
    public int AlunoId { get; set; }

    public int CursoId { get; set; }

    public DateTime DataMatricula { get; set; }

    public string Status { get; set; } = string.Empty;
}