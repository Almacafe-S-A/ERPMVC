using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long Id { get; set; } // bigint
        [Display(Name = "Nombre ordenado")]
        public string SortName { get; set; } // text
        [Display(Name = "País")]
        public string Name { get; set; } // text
        [Display(Name = "Código de area teléfono")]
        public int? PhoneCode { get; set; } // integer
        [Display(Name = "Usuario de creación")]
        public string Usuariocreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string Usuariomodificacion { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }
    }


}
