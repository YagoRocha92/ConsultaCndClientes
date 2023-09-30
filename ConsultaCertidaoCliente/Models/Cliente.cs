using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaCertidaoCliente.Modelos
{
    internal class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CnpjCpf { get; set; }
    }
}
