using ConsultaCertidaoCliente.Data;
using ConsultaCertidaoCliente.Modelos;

namespace ConsultaCertidaoCliente
{
    internal class Program
    {
        private static DALConsulta dal = new DALConsulta();

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

                            dal.InserirCliente();
                            Console.ReadLine();

                            break;
                        case 2:

                            dal.AtualizarDadosCliente();
                            Console.ReadLine();
                            break;
                        case 3:
                            dal.ListarTodosClientes();
                            Console.ReadLine();
                            break;
                        case 4:
                            Console.WriteLine("Digite o nome ou CNPJ/CPF para pesquisa:");
                            string termoPesquisa = Console.ReadLine();
                            dal.PesquisarClientesPorNomeOuCnpjCpf(termoPesquisa);
                            Console.ReadLine();
                            break;
                        case 5:
                            Console.WriteLine("Digite o ID do cliente que deseja excluir:");
                            if (int.TryParse(Console.ReadLine(), out int clienteIdExclusao))
                            {
                                dal.ExcluirCliente(clienteIdExclusao);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID de cliente inválido. Pressione Enter para continuar.");
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
                            dal.InserirCertidao();
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Digite o ID do cliente que deseja atualizar a certidão:");
                            if (int.TryParse(Console.ReadLine(), out int clienteId))
                            {
                                dal.AtualizarDadosCertidao(clienteId);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID de certidão inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;
                        case 3:
                            dal.ListarTodasCertidoesPorOrdemDeValidade();
                            Console.ReadLine();
                            break;
                        case 4:
                            Console.WriteLine("Digite o ID do cliente para listar suas certidões:");
                            if (int.TryParse(Console.ReadLine(), out int clienteIdCertidoes))
                            {
                                dal.ListarCertidoesPorCliente(clienteIdCertidoes);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID de cliente inválido. Pressione Enter para continuar.");
                                Console.ReadLine();
                            }
                            break;

                        case 5:
                            Console.WriteLine("Digite o ID da certidão que deseja excluir:");
                            if (int.TryParse(Console.ReadLine(), out int certidaoIdExclusao))
                            {
                                dal.ExcluirCertidao(certidaoIdExclusao);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("ID de certidão inválido. Pressione Enter para continuar.");
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