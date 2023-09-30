using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaCertidaoCliente.Modelos
{
    internal class CertificadoDigital
    {
        public int Id { get; set; }
        public DateTime DataValidade { get; set; }
        public string Tipo { get; set; }
    }
}
