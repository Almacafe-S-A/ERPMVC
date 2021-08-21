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
        public Liquidacion Liqudacion { get; set; }

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

        public decimal ValorDerechosImportacion { get; set; }

        public decimal TotalCIFDerechosImp { get; set; }

        public decimal ValorSelectivoConsumo { get; set; }

        public decimal? ValorUnitarioDerechos { get; set; }

        public decimal TotalDerechos { get; set; }

        public decimal OtrosImpuestos { get; set; }

        public decimal TotalImpuestoVentas { get; set; }

        public decimal TotalDerechosmasImpuestos { get; set; }

        public decimal TotalFinal { get; set; }

        public decimal? PrecioUnitarioCIF { get; set; }

        public decimal? ValorTotalDerechos { get; set; }

        public decimal? ValorTotalCIF { get; set; }





    }
}
