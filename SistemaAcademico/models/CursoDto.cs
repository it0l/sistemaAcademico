namespace SistemaAcademico.Models;

public class CursoDto
{
    public string Nome { get; set; } = string.Empty;

    public string Professor { get; set; } = string.Empty;

    public int CargaHoraria { get; set; }
}