using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class BlackListCustomersDTO:BlackListCustomers
    {
        public List<Branch> _BlacklistCustomers { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
