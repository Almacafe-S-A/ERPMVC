using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Fecha de inicio")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha de fin")]
        public DateTime? EndDate { get; set; }

    }
}
