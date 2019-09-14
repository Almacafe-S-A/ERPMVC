using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Vendor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 VendorId { get; set; }
        [Required]
        public string VendorName { get; set; }
        [Display(Name = "Vendor Type")]
        public Int64 VendorTypeId { get; set; }
        [ForeignKey("VendorTypeId")]
        public VendorType VendorType { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Ciudad")]
        public string City { get; set; }
        [Display(Name = "Departamento")]
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Telefono")]
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Required]
        public string UsuarioCreacion { get; set; }
        [Required]
        public string UsuarioModificacion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public DateTime FechaModificacion { get; set; }
        [Required]
        [Display(Name = "RTN del Proveedor")]
        public string RTN { get; set; }
        [Required]
        [Display(Name = "Identidad del contacto")]
        public string Identidad { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [Required]
        [Display(Name = "Monto Minimo")]
        public double QtyMin { get; set; }
        [Required]
        [Display(Name = "Monto Mensual")]
        public double QtyMonth { get; set; }
        [Display(Name = "Telefono Referencia")]
        public string PhoneReferenceone { get; set; }

        [Required]
        [Display(Name = "Empresa Referencia")]
        public string CompanyReferenceone { get; set; }
        [Display(Name = "Telefono Referencia")]
        public string PhoneReferencetwo { get; set; }

        [Required]
        [Display(Name = "Empresa Referencia")]
        public string CompanyReferencetwo { get; set; }
        [Display(Name = "Activo/Inactivo ")]
        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }


    }
}
