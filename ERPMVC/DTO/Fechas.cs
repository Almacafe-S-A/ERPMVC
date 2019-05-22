using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class Fechas
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public Int64 Id { get; set; }
    }
}
