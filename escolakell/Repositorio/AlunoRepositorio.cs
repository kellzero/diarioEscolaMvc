using EscolaApp.Repositorio;
using escolakell.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace escolakell.Repositorio
{
    public class AlunoRepositorio
    {
        private readonly string _connectionString;
        private readonly ILogger<AlunoRepositorio> _logger;

        public AlunoRepositorio(IConfiguration configuration, ILogger<AlunoRepositorio> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public List<Aluno> ListarTodos()
        {
            var lista = new List<Aluno>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, Nome, AnoNascimento, Turma
                              FROM Alunos
                              ORDER BY Nome";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ano = (int)reader["AnoNascimento"];
                        lista.Add(new Aluno
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            DataNascimento = new DateTime(ano, 1, 1),
                            AnoNascimento = ano, 
                            Turma = reader["Turma"].ToString(),
                        });
                    }
                }
            }
            return lista;
        }

        public int Adicionar(Aluno aluno)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Alunos (Nome, AnoNascimento, Turma) 
                               VALUES (@Nome, @AnoNascimento, @Turma);
                               SELECT SCOPE_IDENTITY();"; 

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@AnoNascimento", aluno.DataNascimento.Year);
                cmd.Parameters.AddWithValue("@Turma", aluno.Turma);

                conn.Open();
               
                int novoId = Convert.ToInt32(cmd.ExecuteScalar());
                return novoId; 
            }
        }

        public Aluno ObterPorId(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, Nome, AnoNascimento, Turma
                               FROM Alunos 
                               WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int ano = (int)reader["AnoNascimento"];
                        return new Aluno
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            DataNascimento = new DateTime(ano, 1, 1),
                            AnoNascimento = ano,
                            Turma = reader["Turma"].ToString(),
                        };
                    }
                }
            }
            return null;
        }

        public void Atualizar(Aluno aluno)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE Alunos 
                               SET Nome = @Nome,
                                   AnoNascimento = @AnoNascimento,
                                   Turma = @Turma
                               WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", aluno.Id);
                cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@AnoNascimento", aluno.DataNascimento.Year);
                cmd.Parameters.AddWithValue("@Turma", aluno.Turma);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Alunos WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}