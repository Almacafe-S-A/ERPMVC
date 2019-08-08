using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Dimensions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = " Numero")]
        [Required]
        [StringLength(30)]
        public string Num { get; set; }
        [Required]
        public int DimCode { get; set; }
        [StringLength(60)]
        public string Description { get; set; }
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
