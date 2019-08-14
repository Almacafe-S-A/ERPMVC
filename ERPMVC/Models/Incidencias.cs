using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPMVC.Models
{
    public class Incidencias
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdIncidencia { get; set; }
        [Display(Name = "Fecha de inicio")]
        public DateTime? FechaInicio { get; set; }
        [Display(Name = "Fecha de fin")]
        public DateTime? FechaFin { get; set; }
        public long? IdEmpleado { get; set; }
        [Display(Name = "Descripción de incidencia")]
        public string DescripcionIncidencia { get; set; }
        [Display(Name = "Tipo de incidencia")]
        public long? IdTipoIncidencia { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
