using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ContactPerson
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 ContactPersonId { get; set; }
        [Display(Name = "Nombre")]
        public string ContactPersonName { get; set; }

        [Display(Name = "IdProveedor")]
        public Int64 VendorId { get; set; }
        [Display(Name = "IdCliente")]
        public Int64 CustomerId { get; set; }



        [Display(Name = "Identidad")]
        public string ContactPersonIdentity { get; set; }

        [Display(Name = "Telefono")]
        public string ContactPersonPhone { get; set; }

        [Display(Name = "Ciudad")]
        public int ContactPersonCityId { get; set; }

        [Display(Name = "Ciudad")]
        public string ContactPersonCity { get; set; }

        [EmailAddress]
        public string ContactPersonEmail { get; set; }
        [Display(Name = "Id de estado")]
        public Int64 ContactPersonIdEstado { get; set; }
        [Display(Name = "Estado")]
        public string ContactPersonEstado { get; set; }

        [Display(Name = "Usuario de creacion")]
        public string CreatedUser { get; set; }
        [Display(Name = "Usuario de Modificacion")]
        public string ModifiedUser { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime ModifiedDate { get; set; }


    }
}
