using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Bonificacion
    {
        public long Id { get; set; }
        [Required]
        [UIHint("Employeesddl")]
        public long EmpleadoId { get; set; }
        public string EmpleadoNombre { get; set; }
        [UIHint("TipoBonificacion")]
        [Required]
        public long TipoId { get; set; }
        public double Monto { get; set; }
        public double Cantidad { get; set; }
        public double Valor { get; set; }
        public DateTime FechaBono { get; set; }
        [UIHint("EstadosBonificacion")]
        [Required]
        public long EstadoId { get; set; }

        [UIHint("EstadosBonificacion")]
        [Required]
        public Estados Estado { get; set; }

        [UIHint("Employeesddl")]

        public Employees Empleado { get; set; }

        public string EstadoNombre { get; set; }
        public string TipoBonificacionNombre { get; set; }
        public Int64? PlanillaId { get; set; }
        [ForeignKey("PlanillaId")]

        public Planilla Planilla { get; set; }
        public string NombreQuincena { get; set; }




        public TipoBonificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        [UIHint("Quincena")]
        [Required]
        public long Quincena { get; set; }

    }
}
