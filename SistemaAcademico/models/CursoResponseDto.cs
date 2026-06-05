namespace SistemaAcademico.Models;

public class CursoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Professor { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public List<AlunoResumidoDto> Alunos { get; set; } = new();
}

public class AlunoResumidoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MatriculaNumero { get; set; } = string.Empty;
}