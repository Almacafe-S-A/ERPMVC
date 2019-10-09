using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class KardexLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Linea Id")]
        public Int64 KardexLineId { get; set; }
        [Display(Name = "Kardex Id")]
        public Int64 KardexId { get; set; }

        [Display(Name = "Fecha")]
        public DateTime KardexDate { get; set; }

        [Display(Name = "Fecha")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Bodega")]
        public Int64 WareHouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WareHouseName { get; set; }

        [Display(Name = "Recibo")]
        public Int64 GoodsReceivedId { get; set; }

        [Display(Name = "Estiba")]
        public Int64 ControlEstibaId { get; set; }

        [Display(Name = "Estiba")]
        public string ControlEstibaName { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProducId { get; set; }

        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Producto")]
        public Int64 SubProducId { get; set; }

        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Entrada")]
        public double QuantityEntry { get; set; }

        [Display(Name = "Salida")]
        public double QuantityOut { get; set; }

        [Display(Name = "Entrada de sacos")]
        public double QuantityEntryBags { get; set; }

        [Display(Name = "Salida de sacos")]
        public double QuantityOutBags { get; set; }

        [Display(Name = "Entrada certificado deposito")]
        public double QuantityEntryCD { get; set; }

        [Display(Name = "Salida de certificado de deposito")]
        public double QuantityOutCD { get; set; }

        [Display(Name = "Saldo Certificado depositos")]
        public double TotalCD { get; set; }

        [Display(Name = "Saldo sacos")]
        public double TotalBags { get; set; }

        [Display(Name = "Saldo")]
        public double Total { get; set; }

        [Display(Name = "Entrada/Salida")]
        public Int32 TypeOperationId { get; set; }

        [Display(Name = "Entrada/Salida")]
        public string TypeOperationName { get; set; }

        [Display(Name = "Centro de costos")]
        public Int64 CostCenterId { get; set; }

        [Display(Name = "Centro de costos")]
        public string CostCenterName { get; set; }

    }
}
