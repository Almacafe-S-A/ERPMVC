using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Purch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Proveedor")]
        public Int64 PurchId { get; set; }
        [Required]
        [Display(Name = "Nombre del Proveedor")]
        public string PurchName { get; set; }
        [Required]
        [Display(Name = "RTN del Proveedor")]
        public string RTN { get; set; }
        [Required]
        [Display(Name = "Identidad del dueño")]
        public string Identidad { get; set; }

        [Display(Name = "Tipo de Proveedor")]
        public int PurchTypeId { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Ciudad")]
        public string City { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [Display(Name = "Grupo de Impuestos")]
        public int taxGroup { get; set; }
        [Display(Name = "Estado")]
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        
        
        [Display(Name = "Monto Minimo")]
        public double QtyMin { get; set; }
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


     
        [Required]
        [Display(Name = "Usuario que lo crea")]
        public string CreatedUser { get; set; }

        [Required]
        [Display(Name = "Usuario que lo modifica")]
        public string ModifiedUser { get; set; }

        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Fecha de Modificacion")]
        public DateTime ModifiedDate { get; set; }

    }
}
