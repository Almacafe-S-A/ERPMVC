using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Payroll
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Planilla")]
        public long IdPlanilla { get; set; }
        [Display(Name = "Nombre de planilla")]
        public string NombrePlanilla { get; set; }
        [Display(Name = "Descripción de planilla")]
        public string DescripcionPlanilla { get; set; }
        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? FechaModificacion { get; set; }

    }
}
