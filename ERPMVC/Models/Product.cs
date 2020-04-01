using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 ProductId { get; set; }
       
        [Display(Name = "Servicio")]
        public string ProductName { get; set; }
        [Display(Name = "Código de Servicio")]
        public string ProductCode { get; set; }
        [Display(Name = "Código de barra")]
        public string Barcode { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Url")]
        public string ProductImageUrl { get; set; }
        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Unidad de medida")]
        public int UnitOfMeasureId { get; set; }
        [Display(Name = "Precio de compra")]
        public double DefaultBuyingPrice { get; set; } = 0.0;
        [Display(Name = "Precio de venta")]
        public double DefaultSellingPrice { get; set; } = 0.0;
        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }
        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }
        [Required]
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Required]
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }
        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
    }
}
