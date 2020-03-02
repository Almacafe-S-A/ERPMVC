using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CierresAccountingDTO : CierresAccounting
    {

        public double Debit { get; set; }
        public string Nombre { get; set; }
        public double Credit { get; set; }

        public double TotalDebit { get; set; }

        public bool? estadocuenta { get; set; }
        public double TotalCredit { get; set; }
        public List<CierresAccountingDTO> Children { get; set; } = new List<CierresAccountingDTO>();
    }

    public class CierresAccountingFilter
    {
        public Int64 TypeAccountId { get; set; }
        public bool? estadocuenta { get; set; }
        public Int64 BitacoraCierreContableId { get; set; }
    }


}
