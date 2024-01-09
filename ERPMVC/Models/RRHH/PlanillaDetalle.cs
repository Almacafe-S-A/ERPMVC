using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class PlanillaDetalle
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long PlanillaId { get; set; }
        [ForeignKey("PlanillaId")]
        public Planilla Planilla { get; set; }

        public long EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public Employees Employee { get; set; }

        public string EmpleadoNombre { get; set; }

        public double MontoBruto { get; set; }

        public double Dias { get; set; }

        public double MontoQuincenal { get; set; }

        public decimal SueldoDiario { get; set; }

        public double HorasExtras { get; set; }

        public double Otros { get; set; }

        public double TotalIngreso { get; set; }

        public double IHSS { get; set; }

        public double RAP { get; set; }

        public double ISR { get; set; }

        //public double Prestamo { get; set; }

        //public double Adelantos { get; set; }

        //public double Bantrab { get; set; }

        public double TotalDeducciones { get; set; }

        public double TotalBonificaciones { get; set; }

        public double MontoNeto { get; set; }


        public List<PlanillaDeduccion> Deducciones { get; set; }

        //public List<deduccion> DeduccionEmpleado { get; set; }

        public List<Bonificacion> Bonificaciones { get; set; }

        //public int MyProperty { get; set; }


        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public string UsuarioCreacion { get; set; }
    }
}
