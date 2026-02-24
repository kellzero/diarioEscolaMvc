namespace escolakell.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Turma {  get; set; }
        public DateTime DataNascimento { get; set; }
        public int AnoNascimento { get; set; }
        public int Idade
        {
            get
            {

                if (DataNascimento == default)
                    return 0;

                var hoje = DateTime.Today;
                var idade = hoje.Year - DataNascimento.Year;


                if (DataNascimento.Date > hoje.AddYears(-idade))
                    idade--;

                return idade;
            }
        }
        public List<Diario> Diarios { get; set; } = new List<Diario>();
    }
}
