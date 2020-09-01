using System;
using System.Collections.Generic;
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
        public DateTime FechaBono { get; set; }
        public long EstadoId { get; set; }
        public Estados Estado { get; set; }
        public Employees Empleado { get; set; }
        public TipoBonificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
