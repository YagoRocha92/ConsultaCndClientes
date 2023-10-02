using ConsultaCertidaoCliente.Modelos;
using ConsultaCertidaoCliente.Utilitarios;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaCertidaoCliente.Data
{
    internal class DalCertidao
    {
        public void InserirCertidao(Certidao certidao)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    try
                    {
                        string sql = "INSERT INTO Certidoes (ClienteId, Tipo, DataValidade) VALUES (@ClienteId, @Tipo, @DataValidade)";
                        using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ClienteId", certidao.ClienteId);
                            command.Parameters.AddWithValue("@Tipo", certidao.TipoCertidao);
                            command.Parameters.AddWithValue("@DataValidade", certidao.DataValidade);

                            int rowsUpdated = command.ExecuteNonQuery();

                            if (rowsUpdated > 0)
                            {
                                Console.WriteLine("Certidão Cadastrada com sucesso.");
                            }
                            else
                            {
                                Console.WriteLine("Erro: Certidão não foi cadastrada, tente novamente");
                            }
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Erro SQL ao inserir certidão: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro geral ao inserir certidão: " + ex.Message);
                    }


                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao abrir a conexão: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao inserir certidão: " + ex.Message);
            }
        }

        public List<Certidao> ListarTodasCertidoesPorOrdemDeValidade()
        {
            List<Certidao> certidaoTodas = new List<Certidao>();
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT c.Nome AS NomeCliente, cr.Tipo AS TipoCertidao, cr.DataValidade FROM Clientes c " +
                                 "INNER JOIN Certidoes cr ON c.Id = cr.ClienteId " +
                                 "ORDER BY cr.DataValidade";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Certidões por Ordem de Validade:");

                            while (reader.Read())
                            {
                                Certidao certidao = new Certidao
                                {
                                    NomeCliente = reader["NomeCliente"].ToString(),
                                    TipoCertidao = reader["TipoCertidao"].ToString(),
                                    DataValidade = Convert.ToDateTime(reader["DataValidade"])
                                };
                                certidaoTodas.Add(certidao);

                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar certidões: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar certidões: " + ex.Message);
            }
            return certidaoTodas;
        }

        public void ExcluirCertidao(int certidaoId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    // Primeiro, verifique se a certidão existe
                    string selectSql = "SELECT Id FROM Certidoes WHERE Id = @CertidaoId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CertidaoId", certidaoId);

                        int existingCertidaoId = Convert.ToInt32(selectCommand.ExecuteScalar());

                        if (existingCertidaoId > 0)
                        {
                            // Exclua a certidão
                            string deleteSql = "DELETE FROM Certidoes WHERE Id = @CertidaoId";
                            using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteSql, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@CertidaoId", certidaoId);

                                int rowsDeleted = deleteCommand.ExecuteNonQuery();

                                if (rowsDeleted > 0)
                                {
                                    Console.WriteLine("Certidão excluída com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Nenhuma certidão foi excluída.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Certidão não encontrada.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao excluir certidão: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao excluir certidão: " + ex.Message);
            }
        }

        public List<Certidao> ListarCertidoesPorCliente(int clienteId)
        {
            List<Certidao> certidoes = new List<Certidao>();

            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string selectSql = "SELECT Id, Tipo, DataValidade FROM Certidoes WHERE ClienteId = @ClienteId ORDER BY DataValidade";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", clienteId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Certidao certidao = new Certidao
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    TipoCertidao = reader.GetString(reader.GetOrdinal("Tipo")),
                                    DataValidade = reader.GetDateTime(reader.GetOrdinal("DataValidade"))
                                };

                                certidoes.Add(certidao);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar certidões do cliente: " + ex.Message);
            }

            return certidoes;
        }

        public void AtualizarDadosCertidao(int clienteId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string selectSql = "SELECT Id, Tipo, DataValidade FROM Certidoes WHERE ClienteId = @ClienteId ORDER BY DataValidade";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", clienteId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine($"Certidões do Cliente (ID: {clienteId}) por Ordem de Validade:");

                            Dictionary<int, string> certidaoIdToTipo = new Dictionary<int, string>();

                           

                            while (reader.Read())
                            {
                                int certidaoId = Convert.ToInt32(reader["Id"]);
                                string tipoCertidao = reader["Tipo"].ToString();
                                DateTime dataValidade = Convert.ToDateTime(reader["DataValidade"]);

                                certidaoIdToTipo[certidaoId] = tipoCertidao;

                                Console.WriteLine($"ID: {certidaoId} - Tipo: {tipoCertidao}, Validade: {dataValidade:dd/MM/yyyy}");
                                
                            }

                            if (certidaoIdToTipo.Count == 0)
                            {
                                Console.WriteLine("Nenhuma certidão encontrada para este cliente.");
                                return;
                            }

                            Console.WriteLine("Digite o número da certidão que deseja atualizar (ou 0 para cancelar):");
                            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha > 0)
                            {
                                if (certidaoIdToTipo.TryGetValue(escolha, out string tipoEscolhido))
                                {
                                    AtualizarCertidaoPorId(clienteId, escolha, tipoEscolhido);
                                }
                                else
                                {
                                    Console.WriteLine("Escolha inválida. Operação de atualização cancelada.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Operação de atualização cancelada.");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar certidões do cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar certidões do cliente: " + ex.Message);
            }
        }
        public void AtualizarCertidaoPorId(int clienteId, int certidaoId, string tipoCertidao)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string selectSql = "SELECT Tipo, DataValidade FROM Certidoes WHERE Id = @CertidaoId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CertidaoId", certidaoId);

                        using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string tipoAtual = reader["Tipo"].ToString();
                                DateTime dataValidadeAtual = Convert.ToDateTime(reader["DataValidade"]);

                                Console.WriteLine($"Dados atuais da Certidão (ID: {certidaoId}):");
                                Console.WriteLine($"Tipo: {tipoAtual}");
                                Console.WriteLine($"Data de Validade: {dataValidadeAtual:dd/MM/yyyy}");
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Certidão não encontrada. Operação de atualização cancelada.");
                                return;
                            }
                        }
                    }

                    Console.WriteLine("Digite o novo tipo da certidão:");
                    string novoTipo = Console.ReadLine();

                    Console.WriteLine("Digite a nova data de validade da certidão (dd/mm/yyyy):");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime novaDataValidade))
                    {
                        string updateSql = "UPDATE Certidoes SET Tipo = @NovoTipo, DataValidade = @NovaDataValidade WHERE Id = @CertidaoId";
                        using (SQLiteCommand updateCommand = new SQLiteCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@NovoTipo", novoTipo);
                            updateCommand.Parameters.AddWithValue("@NovaDataValidade", novaDataValidade);
                            updateCommand.Parameters.AddWithValue("@CertidaoId", certidaoId);

                            int rowsUpdated = updateCommand.ExecuteNonQuery();

                            if (rowsUpdated > 0)
                            {
                                Console.WriteLine("Dados da certidão atualizados com sucesso.");
                            }
                            else
                            {
                                Console.WriteLine("Nenhuma certidão foi atualizada.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Data inválida. Nenhuma certidão foi atualizada.");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao atualizar dados da certidão: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao atualizar dados da certidão: " + ex.Message);
            }
        }

        //public List<Certidao> ObterCertidoesPorCliente(int clienteId)
        //{
        //    List<Certidao> certidoes = new List<Certidao>();

        //    try
        //    {
        //        using (SQLiteConnection connection = DbHelper.GetConnection())
        //        {
        //            connection.Open();

        //            string selectSql = "SELECT Id, Tipo, DataValidade FROM Certidoes WHERE ClienteId = @ClienteId ORDER BY DataValidade";

        //            using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
        //            {
        //                command.Parameters.AddWithValue("@ClienteId", clienteId);

        //                using (SQLiteDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Certidao certidao = new Certidao
        //                        {
        //                            Id = Convert.ToInt32(reader["Id"]),
        //                            TipoCertidao = reader["Tipo"].ToString(),
        //                            DataValidade = Convert.ToDateTime(reader["DataValidade"])
        //                        };

        //                        certidoes.Add(certidao);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro SQL ao obter certidões do cliente: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Erro geral ao obter certidões do cliente: " + ex.Message);
        //    }

        //    return certidoes;
        //}

        //public Certidao ObterCertidaoPorId(int certidaoId)
        //{
        //    Certidao certidao = null;

        //    try
        //    {
        //        using (SQLiteConnection connection = DbHelper.GetConnection())
        //        {
        //            connection.Open();

        //            string selectSql = "SELECT Tipo, DataValidade FROM Certidoes WHERE Id = @CertidaoId";

        //            using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
        //            {
        //                command.Parameters.AddWithValue("@CertidaoId", certidaoId);

        //                using (SQLiteDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        certidao = new Certidao
        //                        {
        //                            Id = certidaoId,
        //                            TipoCertidao = reader["Tipo"].ToString(),
        //                            DataValidade = Convert.ToDateTime(reader["DataValidade"])
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro SQL ao obter certidão por ID: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Erro geral ao obter certidão por ID: " + ex.Message);
        //    }

        //    return certidao;
        //}

        //public void AtualizarCertidao(Certidao certidao)
        //{
        //    try
        //    {
        //        using (SQLiteConnection connection = DbHelper.GetConnection())
        //        {
        //            string updateSql = "UPDATE Certidoes SET Tipo = @NovoTipo, DataValidade = @NovaDataValidade WHERE Id = @CertidaoId";
        //            using (SQLiteCommand updateCommand = new SQLiteCommand(updateSql, connection))
        //            {
        //                updateCommand.Parameters.AddWithValue("@NovoTipo", certidao.TipoCertidao);
        //                updateCommand.Parameters.AddWithValue("@NovaDataValidade", certidao.DataValidade);
        //                updateCommand.Parameters.AddWithValue("@CertidaoId", certidao.Id);

        //                int rowsUpdated = updateCommand.ExecuteNonQuery();

        //                if (rowsUpdated <= 0)
        //                {
        //                    throw new Exception("Nenhuma certidão foi atualizada.");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao atualizar a certidão: " + ex.Message);
        //    }
        //}
    }
}