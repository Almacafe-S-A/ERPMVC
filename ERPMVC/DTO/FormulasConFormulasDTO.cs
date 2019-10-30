using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class FormulasConFormulasDTO : FormulasConFormulas
    {
        public List<FormulasConFormulas> _FormulasConFormulas { get; set; }
        public Int64 editar { get; set; } = 1;
    }
}
