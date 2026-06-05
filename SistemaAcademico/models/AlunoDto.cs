namespace SistemaAcademico.Models;

public class AlunoDto
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MatriculaNumero { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }
}