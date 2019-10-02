using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class AccountingDTO : Accounting
    {

        public double Debit { get; set; }
        public string Nombre { get; set; }
        public double Credit { get; set; }

        public double TotalDebit { get; set; }

        public double TotalCredit { get; set; }
        public List<AccountingDTO> Children { get; set; } = new List<AccountingDTO>();
    }
}
