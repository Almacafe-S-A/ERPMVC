using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class MantenimientoImpacto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MantenimientoImpactoId { get; set; }

        [Display(Name = "Número de Impacto")]
        public Int64 NoImpacto { get; set; }
        [Display(Name = "Rango")]
        public string Rango { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
    }
}
