using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class UnitOfMeasure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Display(Name = "Unidad de medida")]
        public int UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }
        [Required]
        public string UsuarioModificacion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public DateTime FechaModificacion { get; set; }

        
    }
}
