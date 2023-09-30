using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaCertidaoCliente.Modelos
{
    internal class Certidao
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string TipoCertidao { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataValidade { get; set; }


    }
}
