using escolakell.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EscolaApp.Repositorio
{
    public class DiarioRepositorio
    {
        private readonly string _connectionString;
        private readonly ILogger<DiarioRepositorio> _logger;

        public DiarioRepositorio(IConfiguration configuration, ILogger<DiarioRepositorio> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        private bool CharParaBool(string valor) => valor == "S";



        public List<Diario> ListarTodosComAluno()
        {
            var lista = new List<Diario>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT L.Id, L.Materia, L.Nota1, L.Nota2, L.Media, L.SituacaoAprovada, 
                                      L.AlunoId,
                                      C.Id as Aluno_Id,
                                      C.Nome as Aluno_Nome,
                                      C.Turma as Aluno_Turma,
                                      C.AnoNascimento as Aluno_AnoNascimento
                               FROM Diarios L
                               INNER JOIN Alunos C ON L.AlunoId = C.Id
                               ORDER BY C.Nome, L.Materia";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var diario = new Diario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Materia = reader["Materia"].ToString(),
                            Nota1 = Convert.ToDouble(reader["Nota1"]),
                            Nota2 = Convert.ToDouble(reader["Nota2"]),
                            Media = Convert.ToDouble(reader["Media"]),
                            SituacaoAprovada = CharParaBool(reader["SituacaoAprovada"].ToString()),
                            AlunoId = Convert.ToInt32(reader["AlunoId"])
                        };

                        // Carrega o aluno relacionado
                        if (reader["Aluno_Nome"] != DBNull.Value)
                        {
                            diario.Aluno = new Aluno
                            {
                                Id = Convert.ToInt32(reader["Aluno_Id"]),
                                Nome = reader["Aluno_Nome"].ToString(),
                                Turma = reader["Aluno_Turma"].ToString(),
                                AnoNascimento = Convert.ToInt32(reader["Aluno_AnoNascimento"])
                            };
                        }

                        lista.Add(diario);
                    }
                }
            }

            return lista;
        }

        public List<Diario> ListarTodos()
        {
            var lista = new List<Diario>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, Materia, Nota1, Nota2, Media, SituacaoAprovada, AlunoId
                               FROM Diarios
                               ORDER BY Materia";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Diario
                        {
                            Id = (int)reader["Id"],
                            Materia = reader["Materia"].ToString(),
                            Nota1 = Convert.ToDouble(reader["Nota1"]),
                            Nota2 = Convert.ToDouble(reader["Nota2"]),
                            Media = Convert.ToDouble(reader["Media"]),
                            SituacaoAprovada = CharParaBool(reader["SituacaoAprovada"].ToString()),
                            AlunoId = Convert.ToInt32(reader["AlunoId"])
                        });
                    }
                }
            }
            return lista;
        }

        public Diario ObterPorId(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, Materia, Nota1, Nota2, Media, SituacaoAprovada, AlunoId
                               FROM Diarios 
                               WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Diario
                        {
                            Id = (int)reader["Id"],
                            AlunoId = (int)reader["AlunoId"],
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

        public Diario ObterUltimaNotaPorAlunoId(int alunoId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT TOP 1 Id, AlunoId, Materia, Nota1, Nota2, Media, SituacaoAprovada
                               FROM Diarios 
                               WHERE AlunoId = @AlunoId
                               ORDER BY Id DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AlunoId", alunoId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Diario
                        {
                            Id = (int)reader["Id"],
                            AlunoId = (int)reader["AlunoId"],
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


        public Diario ObterPorAlunoIdEMateria(int alunoId, string materia)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT Id, AlunoId, Materia, Nota1, Nota2, Media, SituacaoAprovada
                               FROM Diarios 
                               WHERE AlunoId = @AlunoId AND Materia = @Materia";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AlunoId", alunoId);
                cmd.Parameters.AddWithValue("@Materia", materia);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Diario
                        {
                            Id = (int)reader["Id"],
                            AlunoId = (int)reader["AlunoId"],
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
        public void Atualizar(Diario diario)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE Diarios 
                       SET Materia = @Materia, 
                           Nota1 = @Nota1, 
                           Nota2 = @Nota2, 
                           Media = @Media, 
                           SituacaoAprovada = @SituacaoAprovada
                       WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", diario.Id);
                cmd.Parameters.AddWithValue("@Materia", diario.Materia);
                cmd.Parameters.AddWithValue("@Nota1", diario.Nota1);
                cmd.Parameters.AddWithValue("@Nota2", diario.Nota2);
                cmd.Parameters.AddWithValue("@Media", diario.Media);
                cmd.Parameters.AddWithValue("@SituacaoAprovada", diario.SituacaoAprovada ? "S" : "N");

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void Adicionar(Diario diario)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Diarios (AlunoId, Materia, Nota1, Nota2, Media, SituacaoAprovada) 
                       VALUES (@AlunoId, @Materia, @Nota1, @Nota2, @Media, @SituacaoAprovada)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AlunoId", diario.AlunoId);
                cmd.Parameters.AddWithValue("@Materia", diario.Materia);
                cmd.Parameters.AddWithValue("@Nota1", diario.Nota1);
                cmd.Parameters.AddWithValue("@Nota2", diario.Nota2);
                cmd.Parameters.AddWithValue("@Media", diario.Media);
                cmd.Parameters.AddWithValue("@SituacaoAprovada", diario.SituacaoAprovada ? "S" : "N");

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


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