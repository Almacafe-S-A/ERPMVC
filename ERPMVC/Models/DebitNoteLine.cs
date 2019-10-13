using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class DebitNoteLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Linea Id")]
        public Int64 DebitNoteLineId { get; set; }
        [Display(Name = "Nota de débito")]
        public int DebitNoteId { get; set; }
        [Display(Name = "Nota de débito")]
        public DebitNote DebitNote { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Nombre producto")]
        public string ProductName { get; set; }

        [Display(Name = "SubProducto")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Nombre SubProducto")]
        public string SubProductName { get; set; }

        [Display(Name = "Cuenta")]
        public Int64 AccountId { get; set; }

        [Display(Name = "Cuenta")]
        public string AccountName { get; set; }


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

        public Int64 WareHouseId { get; set; }
        [Display(Name = "Centro de costos")]
        public Int64 CostCenterId { get; set; }

        [Display(Name = "Centro de costos")]
        public string CostCenterName { get; set; }

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

        [Display(Name = "Monto Impuesto")]
        public double TaxAmount { get; set; }
        public double Total { get; set; }
    }
}
