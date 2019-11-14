using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class InsuranceEndorsementDTO : InsuranceEndorsement
    {

        public List<InsuranceEndorsement> _InsuranceEndorsement { get; set; }
        public int editar { get; set; } = 1;
    }
}
