using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ERPMVC.Models
{
    public class DetalleBiometrico
    {
        public long Id { get; set; }

        public long IdBiometrico { get; set; }

        public long IdEmpleado { get; set; }

        public DateTime FechaHora { get; set; }

        public string Tipo { get; set; }

        public long IdHorario { get; set; }

        public bool? SalidaPendiente { get; set; }

        [JsonIgnore]
        public Biometrico Encabezado { get; set; }

        public Employees Empleado { get; set; }

        
    }
}
