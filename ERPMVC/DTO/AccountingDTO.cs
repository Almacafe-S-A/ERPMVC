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

        public double Credit { get; set; }

        public List<AccountingDTO> Children { get; set; } = new List<AccountingDTO>();
    }
}
