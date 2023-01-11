using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class BoletaDeSalidaLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoletaSalidaLineId { get; set; }

        public Int64 BoletaSalidaId { get; set; }
        [ForeignKey("BoletaSalidaId")]
        public BoletaDeSalida BoletaDeSalida { get; set; }

        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }
        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64? UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }
        [ForeignKey("SubProductId")]
        [UIHint("ProductoCliente")]
        public CustomerProduct SubProduct { get; set; }

        
        [ForeignKey("UnitOfMeasureId")]
        [UIHint("UOM")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [Display(Name = "Cantidad")]
        public decimal Quantity { get; set; }
        public int? Warehouseid { get; set; }
        public string WarehouseName { get; set; }
        [ForeignKey("Warehouseid ")]
        [UIHint("Bodega")]
        public Warehouse Warehouse { get; set; }

    }
}
