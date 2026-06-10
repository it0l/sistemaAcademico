using System.Text.Json.Serialization;

namespace SistemaAcademico.Models;

public class Matricula
{
    public int Id { get; set; }

    public int AlunoId { get; set; }
    
    [JsonIgnore]
    public Aluno? Aluno { get; set; }

    public int CursoId { get; set; }
    public Curso? Curso { get; set; }

    public DateTime DataMatricula { get; set; }
    public string Status { get; set; } = string.Empty;
}