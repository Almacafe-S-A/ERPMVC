using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class InsurancesCertificateLineDTO: InsurancesCertificateLine
    {
        public List<InsurancesCertificateLine> _InsurancesCertificateLine { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }



    }
}
