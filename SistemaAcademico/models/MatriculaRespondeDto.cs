namespace SistemaAcademico.Models;

public class MatriculaResponseDto
{
    public int Id { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public string NomeCurso { get; set; } = string.Empty;

    public DateTime DataMatricula { get; set; }

    public string Status { get; set; } = string.Empty;
}