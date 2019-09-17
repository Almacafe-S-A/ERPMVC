using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{

    public class VendorType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Identificacdor")]
        public Int64 VendorTypeId { get; set; }
        [Required]
        [Display(Name = "Tipo de Proveedor")]
        public string VendorTypeName { get; set; }
        [Display(Name = "Decripcion")]
        public string Description { get; set; }
        public string UsuarioCreacion { get; set; }
        [Required]
        public string UsuarioModificacion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public DateTime FechaModificacion { get; set; }
    }
    
}
