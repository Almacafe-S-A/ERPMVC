using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class SalesOrderLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 SalesOrderLineId { get; set; }
        [Display(Name = "Cotizacion Id")]
        public int SalesOrderId { get; set; }
       // [Display(Name = "Cotizacion")]
      //  public SalesOrder SalesOrder { get; set; }
        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }
         [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Sub Servicio")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Nombre Subservicio")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de Medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnitOfMeasureName { get; set; }

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
        public Int64 TaxId { get; set; }

        [Display(Name = "Código Impuesto")]
        public string TaxCode { get; set; }

        [Display(Name = "Monto Impuesto")]
        public double TaxAmount { get; set; }
        public double Total { get; set; }

        [Display(Name = "Valor")]
        public decimal? Valor { get; set; }

        [Display(Name = "Porcentaje")]
        public decimal? Porcentaje { get; set; }

        public int TipoCobroId { get; set; }
        [ForeignKey("TipoCobroId")]
        public ElementoConfiguracion TipoCobro { get; set; }

        public string TipoCobroName { get; set; }

        public double PeriodoCobro { get; set; }
    }
}
