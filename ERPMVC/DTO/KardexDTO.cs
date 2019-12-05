using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class KardexDTO : Kardex
    {
        public List<Int64> Ids { get; set; } = new List<long>();

        public Int64 SalesOrderId { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha de fin")]
        public DateTime? EndDate { get; set; }
    }


}
