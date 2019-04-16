using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPAPI.Models
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

        [Display(Name = "Estado")]
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Identidad del contacto")]
        public string Identidad { get; set; }
        public string Email { get; set; }
        [Display(Name = "Persona de Contacto ")]
        public string ContactPerson { get; set; }

        [Display(Name = "Activo/Inactivo ")]
        public Int64 IdEstado { get; set; }

        public List<CustomersOfCustomer> _Customers { get; set; }

        public List<VendorOfCustomer> _Vendor { get; set; }

    }
}
