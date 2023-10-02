using ConsultaCertidaoCliente.Modelos;
using ConsultaCertidaoCliente.Utilitarios;
using System;
using System.Data;
using System.Data.SQLite;

namespace ConsultaCertidaoCliente.Data
{
    internal class DALConsulta
    {

        // SCRIPT PARA CRIAR AS TABELAS NO BD
        public void CriarTabelas()
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    // Tabela de clientes
                    string sqlClientes = "CREATE TABLE IF NOT EXISTS Clientes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT, CnpjCpf TEXT)";
                    using (SQLiteCommand command = new SQLiteCommand(sqlClientes, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Tabela de certidões
                    string sqlCertidoes = "CREATE TABLE IF NOT EXISTS Certidoes (Id INTEGER PRIMARY KEY AUTOINCREMENT, ClienteId INTEGER, Tipo TEXT, DataValidade DATETIME)";
                    using (SQLiteCommand command = new SQLiteCommand(sqlCertidoes, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Tabela de procuração eletrônica
                    string sqlProcuracaoEletronica = "CREATE TABLE IF NOT EXISTS ProcuracaoEletronica (Id INTEGER PRIMARY KEY AUTOINCREMENT, ClienteId INTEGER, Tipo TEXT, DataValidade DATETIME)";
                    using (SQLiteCommand command = new SQLiteCommand(sqlProcuracaoEletronica, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Tabela de certificados digitais
                    string sqlCertificadosDigitais = "CREATE TABLE IF NOT EXISTS CertificadoDigital (Id INTEGER PRIMARY KEY AUTOINCREMENT, ClienteId INTEGER, Tipo TEXT, NomeTitular TEXT, DataValidade DATETIME)";
                    using (SQLiteCommand command = new SQLiteCommand(sqlCertificadosDigitais, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL: " + ex.Message);
                // Lidar com a exceção de acordo com suas necessidades, como registrar o erro em um arquivo de log ou notificar o usuário.
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral: " + ex.Message);
            }
        }


        // metodos PROCURAÇÃO ELETRONICA (INSERIR, ALTERAR, PESQUISAR E EXCLUIR)
        public void InserirProcuracaoEletronica()
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    Console.WriteLine("Digite o ID do cliente para o qual deseja cadastrar a procuração:");
                    if (int.TryParse(Console.ReadLine(), out int clienteId))
                    {
                        Console.WriteLine("Digite o tipo de procuração (municipal, estadual, federal):");
                        string tipoProcuracao = Console.ReadLine();

                        Console.WriteLine("Digite a data de validade da procuração (dd/mm/yyyy):");
                        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataValidade))
                        {
                            string insertQuery = "INSERT INTO ProcuracaoEletronica (ClienteId, Tipo, DataValidade) VALUES (@ClienteId, @Tipo, @DataValidade);";
                            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                            {
                                cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                                cmd.Parameters.AddWithValue("@Tipo", tipoProcuracao);
                                cmd.Parameters.AddWithValue("@DataValidade", dataValidade);
                                int rowsUpdated = cmd.ExecuteNonQuery();

                                if (rowsUpdated > 0)
                                {
                                    Console.WriteLine("Procuração cadastrada com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Erro: A Procuração não foi cadastrada, tente novamente.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data inválida. A procuração eletrônica não será adicionada.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID do cliente inválido. A procuração eletrônica não será adicionada.");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao inserir procurações: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao inserir procurações: " + ex.Message);
            }
        }
        public void AtualizarDadosProcuracaoEletronica(int clienteId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    // Listar todas as procurações do cliente
                    string selectSql = "SELECT Id, DataValidade, Tipo FROM ProcuracaoEletronica WHERE ClienteId = @ClienteId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ClienteId", clienteId);

                        using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                        {
                            List<int> procuracaoIds = new List<int>();

                            Console.WriteLine($"Procurações do Cliente (ID: {clienteId}):");

                            int index = 1;

                            while (reader.Read())
                            {
                                int procuracaoId = Convert.ToInt32(reader["Id"]);
                                string tipoProcuracao = reader["Tipo"].ToString();
                                DateTime dataValidade = Convert.ToDateTime(reader["DataValidade"]);

                                Console.WriteLine($"{index} - Tipo: {tipoProcuracao}, Validade: {dataValidade:dd/MM/yyyy}");

                                // Manter o ID da procuração para referência futura
                                procuracaoIds.Add(procuracaoId);

                                index++;
                            }

                            Console.WriteLine("Digite o número da procuração que deseja atualizar (ou 0 para sair):");
                            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha >= 1 && escolha <= procuracaoIds.Count)
                            {
                                int procuracaoIdEscolhida = procuracaoIds[escolha - 1]; // O índice é baseado em 1

                                // Obtenha os dados atuais da procuração selecionada
                                string selectProcuracaoSql = "SELECT DataValidade, Tipo FROM ProcuracaoEletronica WHERE Id = @ProcuracaoId";
                                using (SQLiteCommand selectProcuracaoCommand = new SQLiteCommand(selectProcuracaoSql, connection))
                                {
                                    selectProcuracaoCommand.Parameters.AddWithValue("@ProcuracaoId", procuracaoIdEscolhida);

                                    using (SQLiteDataReader procuracaoReader = selectProcuracaoCommand.ExecuteReader())
                                    {
                                        if (procuracaoReader.Read())
                                        {
                                            string tipoAtual = procuracaoReader["Tipo"].ToString();
                                            DateTime dataValidadeAtual = Convert.ToDateTime(procuracaoReader["DataValidade"]);

                                            Console.WriteLine($"Dados atuais da Procuração Eletrônica (ID: {procuracaoIdEscolhida}):");
                                            Console.WriteLine($"Tipo: {tipoAtual}");
                                            Console.WriteLine($"Data de Validade: {dataValidadeAtual:dd/MM/yyyy}");
                                            Console.WriteLine();
                                        }
                                    }
                                }

                                // Solicite ao usuário os novos dados para atualização
                                Console.WriteLine("Digite o novo tipo da procuração:");
                                string novoTipo = Console.ReadLine();

                                Console.WriteLine("Digite a nova data de validade da procuração (dd/mm/yyyy):");
                                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime novaDataValidade))
                                {
                                    // Atualize os dados da procuração eletrônica
                                    string updateSql = "UPDATE ProcuracaoEletronica SET Tipo = @NovoTipo, DataValidade = @NovaDataValidade WHERE Id = @ProcuracaoId";
                                    using (SQLiteCommand updateCommand = new SQLiteCommand(updateSql, connection))
                                    {
                                        updateCommand.Parameters.AddWithValue("@NovoTipo", novoTipo);
                                        updateCommand.Parameters.AddWithValue("@NovaDataValidade", novaDataValidade);
                                        updateCommand.Parameters.AddWithValue("@ProcuracaoId", procuracaoIdEscolhida);

                                        int rowsUpdated = updateCommand.ExecuteNonQuery();

                                        if (rowsUpdated > 0)
                                        {
                                            Console.WriteLine("Dados da procuração eletrônica atualizados com sucesso.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nenhuma procuração eletrônica foi atualizada.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Data inválida. Nenhuma procuração eletrônica foi atualizada.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Escolha inválida ou nenhum número fornecido. Nenhuma procuração será atualizada.");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao atualizar dados da procuração eletrônica: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao atualizar dados da procuração eletrônica: " + ex.Message);
            }
        }
        public void ListarProcuracoesDeCliente(int clienteId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT Id, Tipo, DataValidade FROM ProcuracaoEletronica WHERE ClienteId = @ClienteId ORDER BY DataValidade";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", clienteId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine($"Procurações Eletrônicas do Cliente (ID: {clienteId}) por Ordem de Validade:");

                            while (reader.Read())
                            {
                                int procuracaoId = Convert.ToInt32(reader["Id"]);
                                string tipoAtual = reader["Tipo"].ToString();
                                DateTime dataValidade = Convert.ToDateTime(reader["DataValidade"]);

                                Console.WriteLine($"ID: {procuracaoId} - Procuração: {tipoAtual} - Validade: {dataValidade:dd/MM/yyyy}");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar procurações eletrônicas do cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar procurações eletrônicas do cliente: " + ex.Message);
            }
        }
        public void ListarTodasProcuracoesPorOrdemDeValidade()
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT c.Nome AS NomeCliente, pe.DataValidade, pe.Tipo FROM Clientes c " +
                                 "INNER JOIN ProcuracaoEletronica pe ON c.Id = pe.ClienteId " +
                                 "ORDER BY pe.DataValidade";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Procurações Eletrônicas por Ordem de Validade:");

                            while (reader.Read())
                            {
                                string nomeCliente = reader["NomeCliente"].ToString();
                                string tipoAtual = reader["Tipo"].ToString();
                                DateTime dataValidade = Convert.ToDateTime(reader["DataValidade"]);

                                Console.WriteLine($"Cliente: {nomeCliente} - Tipo: {tipoAtual} - Validade: {dataValidade:dd/MM/yyyy}");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar procurações eletrônicas: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar procurações eletrônicas: " + ex.Message);
            }
        }
        public void ExcluirProcuracaoEletronica(int procuracaoId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    // Primeiro, verifique se a procuração eletrônica existe
                    string selectSql = "SELECT Id FROM ProcuracaoEletronica WHERE Id = @ProcuracaoId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ProcuracaoId", procuracaoId);

                        int existingProcuracaoId = Convert.ToInt32(selectCommand.ExecuteScalar());

                        if (existingProcuracaoId > 0)
                        {
                            // Exclua a procuração eletrônica
                            string deleteSql = "DELETE FROM ProcuracaoEletronica WHERE Id = @ProcuracaoId";
                            using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteSql, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@ProcuracaoId", procuracaoId);

                                int rowsDeleted = deleteCommand.ExecuteNonQuery();

                                if (rowsDeleted > 0)
                                {
                                    Console.WriteLine("Procuração eletrônica excluída com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Nenhuma procuração eletrônica foi excluída.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Procuração eletrônica não encontrada.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao excluir procuração eletrônica: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao excluir procuração eletrônica: " + ex.Message);
            }
        }


        // metodos CERTIFICADO DIGITAL (INSERIR, ALTERAR, PESQUISAR E EXCLUIR)

        public void InserirCertificadoDigital()
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    Console.WriteLine("Digite o ID do cliente para o qual deseja cadastrar o Certificado Digital:");
                    if (int.TryParse(Console.ReadLine(), out int clienteId))
                    {
                        Console.WriteLine("Digite a data de validade do Certificado Digital (dd/mm/yyyy):");
                        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataValidade))
                        {
                            Console.WriteLine("Digite o tipo de Certificado (A1, A3, Token):");
                            string tipoCertificado = Console.ReadLine();

                            string insertQuery = "INSERT INTO CertificadoDigital (ClienteId, DataValidade, TipoCertificado) VALUES (@ClienteId, @DataValidade, @TipoCertificado);";
                            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                            {
                                cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                                cmd.Parameters.AddWithValue("@DataValidade", dataValidade);
                                cmd.Parameters.AddWithValue("@TipoCertificado", tipoCertificado);
                                int rowsUpdated = cmd.ExecuteNonQuery();

                                if (rowsUpdated > 0)
                                {
                                    Console.WriteLine("Certificado cadastrado com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Erro: O Certificado não foi cadastrado, tente novamente.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data inválida. O Certificado Digital não será adicionado.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID do cliente inválido. O Certificado Digital não será adicionado.");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao inserir Certificado Digital: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao inserir Certificado Digital: " + ex.Message);
            }
        }
        public void AtualizarDadosCertificadoDigital(int certificadoId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {

                    string selectSql = "SELECT ClienteId, DataValidade, TipoCertificado FROM CertificadoDigital WHERE Id = @CertificadoId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CertificadoId", certificadoId);

                        using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int clienteId = Convert.ToInt32(reader["ClienteId"]);
                                DateTime dataValidadeAtual = Convert.ToDateTime(reader["DataValidade"]);
                                string tipoCertificadoAtual = reader["TipoCertificado"].ToString();

                                Console.WriteLine($"Dados atuais do Certificado Digital (ID: {certificadoId}):");
                                Console.WriteLine($"Cliente ID: {clienteId}");
                                Console.WriteLine($"Data de Validade: {dataValidadeAtual:dd/MM/yyyy}");
                                Console.WriteLine($"Tipo de Certificado: {tipoCertificadoAtual}");
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Certificado Digital não encontrado.");
                                return;
                            }
                        }
                    }


                    Console.WriteLine("Digite a nova data de validade do Certificado Digital (dd/mm/yyyy):");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime novaDataValidade))
                    {
                        Console.WriteLine("Digite o novo tipo de Certificado (A1, A3, Token):");
                        string novoTipoCertificado = Console.ReadLine();


                        string updateSql = "UPDATE CertificadoDigital SET DataValidade = @NovaDataValidade, TipoCertificado = @NovoTipoCertificado WHERE Id = @CertificadoId";
                        using (SQLiteCommand updateCommand = new SQLiteCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@NovaDataValidade", novaDataValidade);
                            updateCommand.Parameters.AddWithValue("@NovoTipoCertificado", novoTipoCertificado);
                            updateCommand.Parameters.AddWithValue("@CertificadoId", certificadoId);

                            int rowsUpdated = updateCommand.ExecuteNonQuery();

                            if (rowsUpdated > 0)
                            {
                                Console.WriteLine("Dados do Certificado Digital atualizados com sucesso.");
                            }
                            else
                            {
                                Console.WriteLine("Nenhum Certificado Digital foi atualizado.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Data inválida. Nenhum Certificado Digital foi atualizado.");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao atualizar dados do Certificado Digital: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao atualizar dados do Certificado Digital: " + ex.Message);
            }
        }
        public void ListarCertificadosDigitaisDeCliente(int clienteId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT Id, DataValidade, TipoCertificado FROM CertificadoDigital WHERE ClienteId = @ClienteId ORDER BY DataValidade";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", clienteId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine($"Certificados Digitais do Cliente (ID: {clienteId}) por Ordem de Validade:");

                            while (reader.Read())
                            {
                                int certificadoId = Convert.ToInt32(reader["Id"]);
                                DateTime dataValidade = Convert.ToDateTime(reader["DataValidade"]);
                                string tipoCertificado = reader["TipoCertificado"].ToString();

                                Console.WriteLine($"ID: {certificadoId}, Validade: {dataValidade:dd/MM/yyyy}, Tipo: {tipoCertificado}");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar Certificados Digitais do cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar Certificados Digitais do cliente: " + ex.Message);
            }
        }
        public void ListarTodosCertificadosPorOrdemDeValidade()
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    string sql = "SELECT c.Nome AS NomeCliente, cd.DataValidade, cd.TipoCertificado FROM Clientes c " +
                                 "INNER JOIN CertificadoDigital cd ON c.Id = cd.ClienteId " +
                                 "ORDER BY cd.DataValidade";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Certificados Digitais por Ordem de Validade:");

                            while (reader.Read())
                            {
                                string nomeCliente = reader["NomeCliente"].ToString();
                                DateTime dataValidade = Convert.ToDateTime(reader["DataValidade"]);
                                string tipoCertificado = reader["TipoCertificado"].ToString();

                                Console.WriteLine($"Cliente: {nomeCliente}, Validade: {dataValidade:dd/MM/yyyy}, Tipo: {tipoCertificado}");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao listar Certificados Digitais: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao listar Certificados Digitais: " + ex.Message);
            }
        }
        public void ExcluirCertificadoDigital(int certificadoId)
        {
            try
            {
                using (SQLiteConnection connection = DbHelper.GetConnection())
                {
                    // Primeiro, verifique se o Certificado Digital existe
                    string selectSql = "SELECT Id FROM CertificadoDigital WHERE Id = @CertificadoId";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CertificadoId", certificadoId);

                        int existingCertificadoId = Convert.ToInt32(selectCommand.ExecuteScalar());

                        if (existingCertificadoId > 0)
                        {
                            // Exclua o Certificado Digital
                            string deleteSql = "DELETE FROM CertificadoDigital WHERE Id = @CertificadoId";
                            using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteSql, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@CertificadoId", certificadoId);

                                int rowsDeleted = deleteCommand.ExecuteNonQuery();

                                if (rowsDeleted > 0)
                                {
                                    Console.WriteLine("Certificado Digital excluído com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Nenhum Certificado Digital foi excluído.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Certificado Digital não encontrado.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro SQL ao excluir Certificado Digital: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro geral ao excluir Certificado Digital: " + ex.Message);
            }
        }
    }
}
























