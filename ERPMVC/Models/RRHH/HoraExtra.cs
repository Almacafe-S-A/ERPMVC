using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class HoraExtra
    {
        public long Id { get; set; }

        public long IdBiometrico { get; set; }

        public long IdEmpleado { get; set; }

        public int Horas { get; set; }
        public int Minutos { get; set; }

        public Biometrico Encabezado { get; set; }

        public Employees Empleado { get; set; }

        public long IdEstado { get; set; }

        public Estados Estado { get; set; }
    }
}
