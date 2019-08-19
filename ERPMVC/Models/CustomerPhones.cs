using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerPhones
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CustomerPhoneId{ get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

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
