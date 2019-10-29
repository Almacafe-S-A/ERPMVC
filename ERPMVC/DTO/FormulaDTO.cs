using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class FormulaDTO : Formula
    {
        public List<Formula> _Formula { get; set; }
        public Int64 editar { get; set; } = 1;
    }
}
