using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Bonificacion
    {
        public long Id { get; set; }
        public long EmpleadoId { get; set; }
        public long TipoId { get; set; }
        public double Monto { get; set; }
        public double Cantidad { get; set; }
        public DateTime FechaBono { get; set; }
        public long EstadoId { get; set; }

        [UIHint("EstadosBonificacion")]
        public Estados Estado { get; set; }

        [UIHint("Employees")]
        public Employees Empleado { get; set; }

        [UIHint("TipoBonificacion")]
        public TipoBonificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        [UIHint("Quincena")]
        public string NombreQuincena { get; set; }
        public long Quincena { get; set; }

    }
}
