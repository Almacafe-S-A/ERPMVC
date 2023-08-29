using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class LiquidacionLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LiquidacionId { get; set; }
        [ForeignKey("LiquidacionId")]
        public Liquidacion Liquidacion { get; set; }

        public Int64? GoodsReceivedLineId { get; set; }

        [ForeignKey("GoodsReceivedLineId")]
        public GoodsReceivedLine GoodsReceivedLine { get; set; }

        public Int64? SubProductId { get; set; }
        [ForeignKey("SubProductId")]
        public SubProduct SubProduct { get; set; }

        public string SubProductName { get; set; }

        public string UOM { get; set; }

        public decimal? Cantidad { get; set; }

        public decimal CantidadRecibida { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalFOB { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCIB { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal TasaCambio { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCIFLPS { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDerechosImportacion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCIFDerechosImp { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorSelectivoConsumo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorUnitarioDerechos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDerechos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OtrosImpuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalImpuestoVentas { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDerechosmasImpuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalFinal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioUnitarioCIF { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorTotalDerechos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorTotalCIF { get; set; }





    }
}
