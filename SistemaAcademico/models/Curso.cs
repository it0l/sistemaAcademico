namespace SistemaAcademico.Models;

public class Curso
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Professor { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }

    public List<Matricula> Matriculas { get; set; } = new();
}
