using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ISR
    {
        [Display(Name = "Id")]
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "De")]
        public double De { get; set; }
        [Required]
        [Display(Name = "Hasta")]
        public double Hasta { get; set; }
        [Required]
        [Display(Name = "Tramo")]
        public string Tramo { get; set; }
        [Required]
        [Display(Name = "%")]
        public double Porcentaje { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
