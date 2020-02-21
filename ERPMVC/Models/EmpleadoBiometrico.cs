using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EmpleadoBiometrico
    {
        public long EmpleadoId { get; set; }

        public string NombreEmpleado { get; set; }

        public long? BiometricoId { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
