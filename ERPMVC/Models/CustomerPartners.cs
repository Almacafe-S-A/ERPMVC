using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerPartners
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 PartnerId { get; set; }
        [Display(Name = "Nombre")]
        public string PartnerName { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }


        [Display(Name = "Identidad")]
        public string Identidad { get; set; }
        public string RTN { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Display(Name = "Listados sancionados")]
        public string Listados { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
