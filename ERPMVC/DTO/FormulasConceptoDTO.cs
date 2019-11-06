using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class FormulasConceptoDTO : FormulasConcepto
    {
        public List<FormulasConcepto> _FormulasConcepto { get; set; }
        public Int64 editar { get; set; } = 1;
    }
}
