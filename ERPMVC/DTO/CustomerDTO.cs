using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CustomerDTO:Customer
    {        
        public DateTime? StartDate { get; set; }       
        public DateTime? EndDate { get; set; }
    }
}
