using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPMVC.Models
{
    public class Payroll
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPlanilla { get; set; }
        public string NombrePlanilla { get; set; }
        public string DescripcionPlanilla { get; set; }
        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
