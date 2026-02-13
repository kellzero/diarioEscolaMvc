using Microsoft.Data.SqlClient;
using escolakell.Models;

namespace EscolaApp.Repositorio
{
    public class AlunoRepositorio
    {
        private readonly string _connectionString;

        public AlunoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ================ MÉTODOS DE CONVERSÃO ================
        private char BoolParaChar(bool valor) => valor ? 'S' : 'N';
        private bool CharParaBool(char valor) => valor == 'S';
        private bool CharParaBool(string valor) => valor == "S";
        // =====================================================

        // Listar todos (já está correto)
        public List<Aluno> ListarTodos()
        {
            var lista = new List<Aluno>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, Nome, Materia, Nota1, Nota2, Media, SituacaoAprovada,
                              
                              CASE 
                                  WHEN Media  >= 6 THEN 'Aprovado' 
                                  ELSE 'Recuperação' 
                              END as 'SituacaoTexto'
                              FROM Diarios
                              ORDER BY Nome, Materia";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Aluno
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            Materia = reader["Materia"].ToString(),
                            Nota1 = Convert.ToDouble(reader["Nota1"]),
                            Nota2 = Convert.ToDouble(reader["Nota2"]),
                            Media = Convert.ToDouble(reader["Media"]),
                            SituacaoAprovada = CharParaBool(reader["SituacaoAprovada"].ToString())
                        });
                    }
                }
            }
            return lista;
        }

        // Adicionar novo aluno
        public void Adicionar(Aluno aluno)
        {

            aluno.Media = (aluno.Nota1 + aluno.Nota2) / 2;
            aluno.SituacaoAprovada = aluno.Media >= 6;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Diarios (Nome, Materia, Nota1, Nota2, Media, SituacaoAprovada) 
                               VALUES (@Nome, @Materia, @Nota1, @Nota2,@Media, @SituacaoAprovada)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", aluno.Nome ?? "");
                cmd.Parameters.AddWithValue("@Materia", aluno.Materia ?? "");
                cmd.Parameters.AddWithValue("@Nota1", aluno.Nota1);
                cmd.Parameters.AddWithValue("@Nota2", aluno.Nota2);
                cmd.Parameters.AddWithValue("@Media", aluno.Media);
                cmd.Parameters.AddWithValue("@SituacaoAprovada", BoolParaChar(aluno.SituacaoAprovada));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Obter aluno por ID
        public Aluno ObterPorId(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, Nome, Materia, Nota1, Nota2, Media, SituacaoAprovada 
                               FROM Diarios 
                               WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Aluno
                        {
                            Id = (int)reader["Id"],
                            Nome = reader["Nome"].ToString(),
                            Materia = reader["Materia"].ToString(),
                            Nota1 = Convert.ToDouble(reader["Nota1"]),
                            Nota2 = Convert.ToDouble(reader["Nota2"]),
                            Media = Convert.ToDouble(reader["Media"]),
                            SituacaoAprovada = CharParaBool(reader["SituacaoAprovada"].ToString())
                        };
                    }
                }
            }
            return null;
        }

        // Atualizar aluno
        public void Atualizar(Aluno aluno)
        {
            aluno.Media = (aluno.Nota1 + aluno.Nota2) / 2;
            aluno.SituacaoAprovada = aluno.Media >= 6;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE Diarios 
                               SET Nome = @Nome, 
                                   Materia = @Materia, 
                                   Nota1 = @Nota1, 
                                   Nota2 = @Nota2,
                                   Media = @Media,
                                   SituacaoAprovada = @SituacaoAprovada
                               WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", aluno.Id);
                cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@Materia", aluno.Materia);
                cmd.Parameters.AddWithValue("@Nota1", aluno.Nota1);
                cmd.Parameters.AddWithValue("@Nota2", aluno.Nota2);
                cmd.Parameters.AddWithValue("@Media", aluno.Media);
                cmd.Parameters.AddWithValue("@SituacaoAprovada", BoolParaChar(aluno.SituacaoAprovada));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Excluir aluno
        public void Excluir(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Diarios WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}