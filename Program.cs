using ConsultaCertidaoCliente.Data;
using ConsultaCertidaoCliente.Modelos;

namespace ConsultaCertidaoCliente
{
    internal class Program
    {
        private static DALConsulta dal = new DALConsulta();
        private static DalCliente dalCliente = new DalCliente();
        private static DalCertificado dalCertificado = new DalCertificado();
        private static DalCertidao dalCertidao = new DalCertidao();

        static void Main(string[] args)
        {
            //dal.CriarTabelas();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1. Gerenciar Clientes");
                Console.WriteLine("2. Gerenciar Certidões");
                Console.WriteLine("3. Gerenciar Procurações Eletrônicas");
                Console.WriteLine("4. Gerenciar Certificados Digitais");
                Console.WriteLine("5. Sair");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            MenuClientes();
                            break;
                        case 2:
                            MenuCertidoes();
                            break;
                        case 3:
                            MenuProcuracoesEletronicas();
                            break;
                        case 4:
                            MenuCertificadosDigitais();
                            break;
                        case 5:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                    Console.ReadLine();
                }
            }
        }


        static void MenuClientes()

        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Gerenciamento de Clientes");
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1. Inserir Cliente");
                Console.WriteLine("2. Atualizar Dados do Cliente");
                Console.WriteLine("3. Listar Todos os Clientes");
                Console.WriteLine("4. Pesquisar Clientes por Nome ou CNPJ/CPF");
                Console.WriteLine("5. Excluir Cliente");
                Console.WriteLine("6. Voltar ao Menu Principal");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:

                            InserirCliente();
                            Console.ReadLine();
                            break;
                        case 2:
                            AtualizarDadosCliente();
                            Console.ReadLine();
                            break;
                        case 3:
                            ListarTodosClientes();
                            Console.ReadLine();
                            break;
                        case 4:
                            PesquisarClientesPorNomeOuCnpjCpf();
                            Console.ReadLine();
                            break;
                        case 5:
                            ExcluirClienteId();
                            Console.ReadLine();
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                    Console.ReadLine();
                }
            }
        }// ok
        static void InserirCliente()
        {
            Console.WriteLine("Digite o nome do cliente: ");
            string nome = Console.ReadLine();
            Console.WriteLine("Digite o CNPJ/CPF do cliente: ");
            string cnpjCpf = Console.ReadLine();

            Cliente novoCliente = new Cliente
            {
                Nome = nome,
                CnpjCpf = cnpjCpf
            };
            dalCliente.InserirCliente(novoCliente);


        } // OK
        static void ListarTodosClientes()
        {
            List<Cliente> clientes = dalCliente.ListarTodosClientes();
            Console.WriteLine("Lista de Clientes:");
            foreach (var cliente in clientes)
            {
                Console.WriteLine($"ID: {cliente.Id}, Nome: {cliente.Nome}, CNPJ/CPF: {cliente.CnpjCpf}");
            }
        }// OK
        static void AtualizarDadosCliente()
        {
            try
            {
                Console.WriteLine("Digite o ID do cliente que deseja atualizar:");
                if (int.TryParse(Console.ReadLine(), out int clienteId))
                {
                    Cliente cliente = dalCliente.ObterClientePorId(clienteId);
                    if (cliente != null)
                    {
                        Console.WriteLine("Digite o novo nome do cliente:");
                        string novoNome = Console.ReadLine();
                        Console.WriteLine("Digite o novo CNPJ/CPF do cliente:");
                        string novoCnpjCpf = Console.ReadLine();

                        cliente.Nome = novoNome;
                        cliente.CnpjCpf = novoCnpjCpf;

                        dalCliente.AtualizarCliente(cliente);
                    }
                    else
                    {
                        Console.WriteLine("Cliente não encontrado.");
                    }
                }
                else
                {
                    Console.WriteLine("ID de cliente inválido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar dados do cliente: " + ex.Message);
            }
        } // OK
        static void PesquisarClientesPorNomeOuCnpjCpf()
        {
            Console.WriteLine("Digite o nome ou CNPJ/CPF para pesquisa:");
            string termoPesquisa = Console.ReadLine();
            List<Cliente> clientes = dalCliente.PesquisarClientesPorNomeOuCnpjCpf(termoPesquisa);

            if (clientes.Count > 0)
            {
                Console.WriteLine("Resultados da Pesquisa:");

                // Use um foreach para percorrer e exibir cada cliente
                foreach (Cliente cliente in clientes)
                {
                    Console.WriteLine($"ID: {cliente.Id}, Nome: {cliente.Nome}, CNPJ/CPF: {cliente.CnpjCpf}");
                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente encontrado com base no critério de pesquisa.");
            }


        } // OK
        static void ExcluirClienteId()
        {
            Console.WriteLine("Digite o ID do cliente que deseja excluir:");

            if (int.TryParse(Console.ReadLine(), out int clienteIdExclusao))
            {
                dalCliente.ExcluirCliente(clienteIdExclusao);
            }
            else
            {
                Console.WriteLine("Tipo de Id invalido, tente novamente");
            }

        }// OK


        static void MenuCertidoes()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Gerenciamento de Certidões");
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1. Inserir Certidão");
                Console.WriteLine("2. Atualizar Dados da Certidão");
                Console.WriteLine("3. Listar todas Certidões por Ordem de Validade");
                Console.WriteLine("4. Listar Certidões de um Cliente");
                Console.WriteLine("5. Excluir Certidão");
                Console.WriteLine("6. Voltar ao Menu Principal");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            InserirCertidao();
                            Console.ReadLine();
                            break;
                        case 2:
                            AtualizarDadosCertidao();
                            Console.ReadLine();
                            break;
                        case 3:
                            ListarTodasCertidoesPorOrdemDeValidade();
                            Console.ReadLine();
                            break;
                        case 4:
                            ListarCertidoesPorCliente();
                            break;

                        case 5:
                            ExcluirCertidao();
                            Console.ReadLine();
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                    Console.ReadLine();
                }
            }
        } //OK
        static void InserirCertidao()
        {
            Console.WriteLine("Digite o Id do cliente que deseja inserir a Certidão:");
            int clienteId = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o tipo da certidão:");
            string tipoCertidao = Console.ReadLine();

            Console.WriteLine("Digite a data de validade da certidão (dd/mm/yyyy):");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataValidadeCertidao))
            {
                Certidao certidao = new Certidao
                {
                    ClienteId = clienteId,
                    TipoCertidao = tipoCertidao,
                    DataValidade = dataValidadeCertidao
                };
                dalCertidao.InserirCertidao(certidao);
            }

            else
            {
                Console.WriteLine("Data inválida. A certidão não será adicionada.");
            }
        }// OK
        static void ListarTodasCertidoesPorOrdemDeValidade()
        {
            {
                List<Certidao> certidoes = dalCertidao.ListarTodasCertidoesPorOrdemDeValidade();
                Console.WriteLine("Lista de Certidoes:");
                foreach (var certidao in certidoes)
                {
                    Console.WriteLine($"Nome: {certidao.NomeCliente}, TIPO: {certidao.TipoCertidao}, VENCIMENTO: {certidao.DataValidade}");
                }
            }// OK
        }//OK
        static void ListarCertidoesPorCliente()
        {
            Console.WriteLine("Digite o ID do cliente para listar suas certidões:");
            if (int.TryParse(Console.ReadLine(), out int clienteIdCertidoes))
            {
                List<Certidao> certidoes = dalCertidao.ListarCertidoesPorCliente(clienteIdCertidoes);
                Console.WriteLine("Lista de Certidoes:");
                foreach (var certidao in certidoes)
                {
                    Console.WriteLine($"ID Certidão: {certidao.Id}, TIPO: {certidao.TipoCertidao}, VENCIMENTO: {certidao.DataValidade}");
                }
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("ID de cliente inválido. Pressione Enter para continuar.");
                Console.ReadLine();
            }

        } // OK
        static void ExcluirCertidao()
        {
            Console.WriteLine("Digite o ID da certidão que deseja excluir:");
            if (int.TryParse(Console.ReadLine(), out int certidaoIdExclusao))
            {
                dalCertidao.ExcluirCertidao(certidaoIdExclusao);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("ID de certidão inválido. Pressione Enter para continuar.");
                Console.ReadLine();
            }

        } // OK
        static void AtualizarDadosCertidao()
        {
            Console.WriteLine("Digite o Id do cliente que deseja atualizar a certidão: ");
            int clienteId = int.Parse(Console.ReadLine());
            dalCertidao.AtualizarDadosCertidao(clienteId);

        }// OK
        ////static void AtualizarCertidaoPorId(int certidaoId)
        //{
        //    Certidao certidao = dalCertidao.ObterCertidaoPorId(certidaoId);

        //    if (certidao == null)
        //    {
        //        Console.WriteLine("Certidão não encontrada. Operação de atualização cancelada.");
        //        return;
        //    }

        //    Console.WriteLine($"Dados atuais da Certidão (ID: {certidao.Id}):");
        //    Console.WriteLine($"Tipo: {certidao.TipoCertidao}");
        //    Console.WriteLine($"Data de Validade: {certidao.DataValidade:dd/MM/yyyy}");

        //    Console.WriteLine("Digite o novo tipo da certidão:");
        //    string novoTipo = Console.ReadLine();

        //    Console.WriteLine("Digite a nova data de validade da certidão (dd/mm/yyyy):");
        //    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime novaDataValidade))
        //    {
        //        certidao.TipoCertidao = novoTipo;
        //        certidao.DataValidade = novaDataValidade;
        //        dalCertidao.AtualizarCertidao(certidao);
        //        Console.WriteLine("Dados da certidão atualizados com sucesso.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Data inválida. Nenhuma certidão foi atualizada.");
        //    }
        //} //OK

        // FAZER MELHORIAS... 
        static void MenuProcuracoesEletronicas()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu de Gerenciamento de Procurações Eletrônicas");
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1. Inserir Procuração Eletrônica");
                Console.WriteLine("2. Atualizar Dados da Procuração Eletrônica");
                Console.WriteLine("3. Listar Procurações de um Cliente");
                Console.WriteLine("4. Listar Todas as Procurações por Ordem de Validade");
                Console.WriteLine("5. Excluir Procuração Eletrônica");
                Console.WriteLine("6. Voltar ao Menu Principal");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            dal.InserirProcuracaoEletronica();
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Digite o ID do cliente que deseja atualizar procuração eletrônica:");
                            if (int.TryParse(Console.ReadLine(), out int clienteIdAtualizar))
                            {
                                dal.AtualizarDadosProcuracaoEletronica(clienteIdAtualizar);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 3:
                            Console.WriteLine("Digite o ID do cliente para listar as procurações:");
                            if (int.TryParse(Console.ReadLine(), out int clienteId))
                            {
                                dal.ListarProcuracoesDeCliente(clienteId);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 4:
                            dal.ListarTodasProcuracoesPorOrdemDeValidade();
                            Console.ReadLine();
                            break;
                        case 5:
                            Console.WriteLine("Digite o ID da procuração eletrônica que deseja excluir:");
                            if (int.TryParse(Console.ReadLine(), out int procuracaoIdExcluir))
                            {
                                dal.ExcluirProcuracaoEletronica(procuracaoIdExcluir);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                    Console.ReadLine();
                }
            }
        }

        static void MenuCertificadosDigitais()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu de Gerenciamento de Certificados Digitais");
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1. Inserir Certificado Digital");
                Console.WriteLine("2. Atualizar Dados do Certificado Digital");
                Console.WriteLine("3. Listar Certificados de um Cliente");
                Console.WriteLine("4. Listar Todos os Certificados por Ordem de Validade");
                Console.WriteLine("5. Excluir Certificado Digital");
                Console.WriteLine("6. Voltar ao Menu Principal");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            dal.InserirCertificadoDigital();
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Digite o ID do Certificado Digital que deseja atualizar:");
                            if (int.TryParse(Console.ReadLine(), out int certificadoId))
                            {
                                dal.AtualizarDadosCertificadoDigital(certificadoId);
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 3:
                            Console.WriteLine("Digite o ID do cliente para listar os certificados:");
                            if (int.TryParse(Console.ReadLine(), out int clienteId))
                            {
                                dal.ListarCertificadosDigitaisDeCliente(clienteId);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 4:
                            dal.ListarTodosCertificadosPorOrdemDeValidade();
                            Console.ReadLine();
                            break;
                        case 5:
                            Console.WriteLine("Digite o ID do Certificado Digital que deseja excluir:");
                            if (int.TryParse(Console.ReadLine(), out int certificadoIdExcluir))
                            {
                                dal.ExcluirCertificadoDigital(certificadoIdExcluir);
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                    Console.ReadLine();
                }
            }
        }
    }
}