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

        [Required]
        public long PlanillaId { get; set; }

        [Required]
        public long EmpleadoId { get; set; }

        public string EmpleadoNombre { get; set; }

        [Required]
        public decimal MontoBruto { get; set; }

        [Required]
        public decimal TotalDeducciones { get; set; }



        [Required]
        public decimal MontoNeto { get; set; }

        //[ForeignKey("DetallePlanillaId")]
        //public List<PlanillaDeduccion> Deducciones { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaModificacion { get; set; }

        [Required]
        public string UsuarioModificacion { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }
    }
}
