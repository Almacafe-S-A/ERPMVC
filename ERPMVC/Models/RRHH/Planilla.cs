using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Planilla
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        public string Comentarios { get; set; }

        public DateTime FechaPlanilla { get; set; }
        public DateTime? FechaPago { get; set; }



        public int PeriodoId { get; set; }
        [ForeignKey("PeriodoId")]
        public Periodo Periodo { get; set; }

        public int Mes { get; set; }

        public int TotalEmpleados { get; set; }

        public double TotalPlanilla { get; set; }

        public long EstadoId { get; set; }

        [ForeignKey("EstadoId")]
        public Estados Estado { get; set; }

        public string EstadoName { get; set; }


        public long TipoPlanillaId { get; set; }

        [ForeignKey("TipoPlanillaId")]
        public PlanillaTipo TipoPlanilla { get; set; }


        public List<PlanillaDetalle> Detalle { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public string UsuarioCreacion { get; set; }
    }
}
