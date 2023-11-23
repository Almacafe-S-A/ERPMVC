using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Inasistencia
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; }

        public long IdEmpleado { get; set; }

        public string Observacion { get; set; }

        public long TipoInasistencia { get; set; }

        public Employees Empleado { get; set; }

        public string NombreEmpleado { get { return Empleado.NombreEmpleado ?? ""; } }
        public string TipoNombre { get { return Tipo.Nombre ?? ""; } }
        public string NombreEstado { get { return Estado.NombreEstado ?? ""; } }

        public long IdEstado { get; set; }

        public Estados Estado { get; set; }

        public ElementoConfiguracion Tipo { get; set; }
        public string HoraLlegada { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
