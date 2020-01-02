using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class VendorInvoiceLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 VendorInvoiceLineId { get; set; }
        [Display(Name = "Factura Proveedor")]
        public int VendorInvoiceId { get; set; }
        [Display(Name = "Factura Proveedor")]
        public VendorInvoice VendorInvoice { get; set; }
        [Display(Name = "Producto Ítem")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Nombre producto")]
        public string ItemName { get; set; }


        [Display(Name = "Unidad de Medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnitOfMeasureName { get; set; }


        [Display(Name = "Descripción")]
        public string Description { get; set; }
          [Display(Name = "Cantidad")]
        public double Quantity { get; set; }
        [Display(Name = "Precio")]
        public double Price { get; set; }
        [Display(Name = "Monto")]
        public double Amount { get; set; }
          [Display(Name = "Porcentaje descuento")]
        public double DiscountPercentage { get; set; }
          [Display(Name = "Monto descuento")]
        public double DiscountAmount { get; set; }
          [Display(Name = "Subtotal")]
        public double SubTotal { get; set; }
         [Display(Name = "% Impuesto")]
        public double TaxPercentage { get; set; }
        [Display(Name = "Código Impuesto")]
        public string TaxCode { get; set; }
        [Display(Name = "Id Impuesto")]
        public Int64? TaxId { get; set; }
        [ForeignKey("TaxId")]
        public virtual Tax Tax { get; set; }
        [Display(Name = "Monto Impuesto")]
        public double TaxAmount { get; set; }
        public double Total { get; set; }

        public long AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Accounting Account { get; set; }
    }
}
