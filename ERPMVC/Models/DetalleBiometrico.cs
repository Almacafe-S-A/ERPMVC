using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class DetalleBiometrico
    {
        public long Id { get; set; }

        public long IdBiometrico { get; set; }

        public long IdEmpleado { get; set; }

        public DateTime FechaHora { get; set; }

        public string Tipo { get; set; }

        public Biometrico Encabezado { get; set; }

        public Employees Empleado { get; set; }
    }
}
