using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace escolakell.Models;
public class Diario
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public string Materia { get; set; }
    public double Nota1 { get; set; }
    public double Nota2 { get; set; }

    public bool SituacaoAprovada { get; set; }

    [Display(Name = "Média")]
    public double Media { get; set; }
    [Display(Name = "Situação")]
    public string SituacaoTexto
    {
        get { return Media >= 6 ? "Aprovado" : "Recuperação"; }
    }

    public Aluno Aluno { get; internal set; }
}