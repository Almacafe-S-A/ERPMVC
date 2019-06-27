using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CustomerProductId { get; set; }
        [Display(Name = "Id de cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Nombre de cliente")]
        public string CustomerName { get; set; }
        [Display(Name = "Id Producto")]
        public Int64 SubProductId { get; set; }
        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Producto")]
        public double SaldoProductoCertificado { get; set; }

        [Display(Name = "Producto")]
        public double SaldoProductoTotal { get; set; }

        [Display(Name = "Activo/Inactivo ")]
        public Int64 IdEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class CustomerTypeSubProduct
    {
        public Int64 CustomerId { get; set; }
        public Int64 SubProductId { get; set; }
        public Int64 ProductTypeId { get; set; }
    }

}
