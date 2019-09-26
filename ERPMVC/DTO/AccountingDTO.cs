using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class AccountingDTO : Accounting
    {

        public List<AccountingDTO> Children = new List<AccountingDTO>();
    }
}
