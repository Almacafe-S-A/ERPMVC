using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class BranchDTO : Branch
    {
        public List<Branch> _Brach { get; set; }

        public Int64 ServicioId { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
