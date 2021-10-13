using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CertificadoLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 CertificadoLineId { get; set; }
        public int? PdaNo { get; set; }
        [Display(Name = "Certificado")]
        public Int64 IdCD { get; set; }
        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }
        [ForeignKey("SubProductId")]
        public SubProduct SubProduct { get; set; }
        [Display(Name = "Nombre producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitMeasureId { get; set; }
        [Display(Name = "Unidad de medida")]
        public string UnitMeasurName { get; set; }

        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }
        [Display(Name = "Precio")]
        [Column(TypeName = "Money")]
        public double Price { get; set; }
        [Display(Name = "Porcentaje de Merma")]
        public double? Merma { get; set; }

        [Display(Name = "Total")]
        [Column(TypeName = "Money")]
        public double Amount { get; set; }


        [Display(Name = "Bodega")]
        public int WarehouseId { get; set; }
        //[ForeignKey("WarehouseId")]
        ///public Warehouse Warehouse { get; set; }
        [Display(Name = "Bodega")]
        public string WarehouseName { get; set; }


        [Display(Name = "Total Cantidad")]
        public double TotalCantidad { get; set; }

        [Display(Name = "Saldo endoso")]
        public double SaldoEndoso { get; set; }

        [Display(Name = "Centro de costos")]
        public Int64 CenterCostId { get; set; }

        [Display(Name = "Centro de costos")]
        public string CenterCostName { get; set; }

        public string Observaciones { get; set; }

        public double? DerechosFiscales { get; set; }
        [NotMapped]
        public int? ReciboId { get; set; }
        [NotMapped]
        public double? CantidadDisponible { get; set; }

    }

}
