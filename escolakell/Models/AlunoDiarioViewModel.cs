// Models/ViewModel/AlunoDiarioViewModel.cs
using escolakell.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace escolakell.Models.ViewModel
{
    public class AlunoDiarioViewModel
    {
        
        [ValidateNever]
        public Aluno Aluno { get; set; }

        
        [ValidateNever]
        public Diario Diario { get; set; }

        
        public AlunoDiarioViewModel()
        {
            Aluno = new Aluno();
            Diario = new Diario();
        }
    }
}