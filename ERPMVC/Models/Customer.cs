using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Cliente")]
        public Int64 CustomerId { get; set; }
        [Required]
        [Display(Name = "Nombre del cliente")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "RTN del cliente")]
        public string RTN { get; set; }

        [Display(Name = "Tipo de cliente")]
        public int CustomerTypeId { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Departamento")]
        public string State { get; set; }
        [Display(Name = "Codigo Zip")]
        public string ZipCode { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Identidad del contacto")]
        public string Identidad { get; set; }
        public string Email { get; set; }
        [Display(Name = "Persona de Contacto ")]
        public string ContactPerson { get; set; }


        [Display(Name = "Activo/Inactivo ")]
        public Int64 IdEstado { get; set; }

        public string Estado { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }

        [Required]
        public string UsuarioModificacion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaModificacion { get; set; }

        //  public List<CustomersOfCustomer> _Customers { get; set; }

        //        public List<VendorOfCustomer> _Vendor { get; set; }

    }
}
