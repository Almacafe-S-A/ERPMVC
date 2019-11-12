using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CertificadoDepositoDTO : CertificadoDeposito
    {
        public List<Int64> RecibosAsociados { get; set; }
        public List<Int64> CertificadosList { get; set; }
        public Int64 SalesOrderId { get; set; }
        public int editar { get; set; } = 1;
       
    }
}
