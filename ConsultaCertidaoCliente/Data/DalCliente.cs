using ConsultaCertidaoCliente.Modelos;
using ConsultaCertidaoCliente.Utilitarios;
using System.Data.SQLite;

namespace ConsultaCertidaoCliente.Data
{
    internal class DalCliente
    {
        public void InserirCliente(Cliente cliente)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string insertQuery = "INSERT INTO Clientes (Nome, CNPJCpf) VALUES (@Nome, @CNPJCpf); SELECT last_insert_rowid();";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                        cmd.Parameters.AddWithValue("@CNPJCpf", cliente.CnpjCpf);

                        int rowsUpdated = cmd.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            Console.WriteLine("Cliente Cadastrado com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Erro: Nenhum cliente foi cadastrado.");
                        }
                    }

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral: " + ex.Message);
            }
        }
        public List<Cliente> PesquisarClientesPorNomeOuCnpjCpf(string termoPesquisa)
        {
            List<Cliente> clientesEncontrados = new List<Cliente>();

            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT Id, Nome, CNPJCpf FROM Clientes WHERE Nome LIKE @TermoPesquisa OR CNPJCpf LIKE @TermoPesquisa";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TermoPesquisa", $"%{termoPesquisa}%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int clienteId = Convert.ToInt32(reader["Id"]);
                                string nomeCliente = reader["Nome"].ToString();
                                string cnpjCpfCliente = reader["CNPJCpf"].ToString();
                                Cliente cliente = new Cliente
                                {
                                    Id = clienteId,
                                    Nome = nomeCliente,
                                    CnpjCpf = cnpjCpfCliente
                                };

                                clientesEncontrados.Add(cliente);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao pesquisar clientes: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao pesquisar clientes: " + ex.Message);
            }
            return clientesEncontrados;
        }
        public List<Cliente> ListarTodosClientes()
        {
            List<Cliente> clientesLista = new List<Cliente>();
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT Id, Nome, CNPJCpf FROM Clientes";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Lista de Todos os Clientes:");

                            while (reader.Read())
                            {
                                int clienteId = Convert.ToInt32(reader["Id"]);
                                string nomeCliente = reader["Nome"].ToString();
                                string cnpjCpfCliente = reader["CNPJCpf"].ToString();

                                Cliente cliente = new Cliente
                                {
                                    Id = clienteId,
                                    Nome = nomeCliente,
                                    CnpjCpf = cnpjCpfCliente
                                };

                                clientesLista.Add(cliente);

                            }
                        }
                    }
                }
                return clientesLista;
            }

            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar todos os clientes: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar todos os clientes: " + ex.Message);
            }
            return clientesLista;
        }
        public Cliente ObterClientePorId(int clienteId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string selectSql = "SELECT Id, Nome, CnpjCpf FROM Clientes WHERE Id = @ClienteId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ClienteId", clienteId);

                        using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Cliente cliente = new Cliente
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                    CnpjCpf = reader.GetString(reader.GetOrdinal("CnpjCpf"))
                                };

                                return cliente;
                            }
                        }
                    }
                }
                return null; // Retorna null se o cliente não for encontrado
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Erro SQL ao consultar cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro geral ao consultar cliente: " + ex.Message);
            }
        }
        public void AtualizarCliente(Cliente cliente)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string updateSql = "UPDATE Clientes SET Nome = @NovoNome, CnpjCpf = @NovoCnpjCpf WHERE Id = @ClienteId";
                    using (SQLiteCommand updateCommand = new SQLiteCommand(updateSql, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@NovoNome", cliente.Nome);
                        updateCommand.Parameters.AddWithValue("@NovoCnpjCpf", cliente.CnpjCpf);
                        updateCommand.Parameters.AddWithValue("@ClienteId", cliente.Id);

                        int rowsUpdated = updateCommand.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            Console.WriteLine("Dados do cliente atualizados com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Nenhum cliente foi atualizado.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Erro SQL ao atualizar dados do cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro geral ao atualizar dados do cliente: " + ex.Message);
            }
        }
        public void ExcluirCliente(int clienteId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string selectSql = "SELECT Id FROM Clientes WHERE Id = @ClienteId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ClienteId", clienteId);

                        int existingClientId = Convert.ToInt32(selectCommand.ExecuteScalar());

                        if (existingClientId > 0)
                        {
                            string deleteSql = "DELETE FROM Clientes WHERE Id = @ClienteId";
                            using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteSql, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@ClienteId", clienteId);

                                int rowsDeleted = deleteCommand.ExecuteNonQuery();

                                if (rowsDeleted > 0)
                                {
                                    Console.WriteLine("Cliente excluído com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Nenhum cliente foi excluído.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Cliente não encontrado.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao excluir cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao excluir cliente: " + ex.Message);
            }
        }
    }
}
