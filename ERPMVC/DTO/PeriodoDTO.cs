using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class PeriodoDTO : Periodo
    {
        public List<Periodo> _Periodo { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }

        public int Periodo { get; set; }

        public int Mes { get; set; }

        public string Mensaje { get; set; }

        public bool bloquearapertura { get; set; }
    }
}
