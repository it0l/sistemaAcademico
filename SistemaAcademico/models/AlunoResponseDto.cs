namespace SistemaAcademico.Models;

public class AlunoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MatriculaNumero { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public List<MatriculaResumidaDto> Matriculas { get; set; } = new();
}

public class MatriculaResumidaDto
{
    public int Id { get; set; }
    public DateTime DataMatricula { get; set; }
    public string Status { get; set; } = string.Empty;
    public CursoResumidoDto? Curso { get; set; }
}

public class CursoResumidoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Professor { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
}