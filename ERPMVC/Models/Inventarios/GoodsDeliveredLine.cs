using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GoodsDeliveredLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Linea Id")]
        public Int64 GoodsDeliveredLinedId { get; set; }
        [Display(Name = "Recibo de mercaderia")]
        public Int64 GoodsDeliveredId { get; set; }

        public int Pda { get; set; }

        [Display(Name = "Número de certificado de depósito")]
        public Int64 NoCD { get; set; }

        [Display(Name = "Número de autorización de retiro")]
        public Int64 NoAR { get; set; }

        [Display(Name = "Unidad de Medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnitOfMeasureName { get; set; }
        
        [Display(Name = "Producto")]
        public Int64 ProducId { get; set; }
        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Producto Cliente")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Producto Cliente")]
        public string SubProductName { get; set; }

        [Display(Name = "Descripcion del producto")]
        public string Description { get; set; }
        [Display(Name = "Estiba")]
        public Int64 ControlPalletsId { get; set; }
        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        public decimal? QuantityAuthorized { get; set; }
        [Display(Name = "Sacos")]
        public int QuantitySacos { get; set; }

        [Display(Name = "Precio")]
        public double Price { get; set; }
        [Display(Name = "Total")]
        public double Total { get; set; }
        [Display(Name = "Bodega")]
        public Int64 WareHouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WareHouseName { get; set; }

        [Display(Name = "Centro de costos")]
        public Int64 CostCenterId { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


    public class GoodsDeliveredLineDTO
    {
        public Int64 GoodsDeliveredLinedId { get; set; }

        [Display(Name = "Recibo de mercaderia")]
        public Int64 GoodsDeliveredId { get; set; }
        [Display(Name = "Unidad de Medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProducId { get; set; }
        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Producto Cliente")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Producto Cliente")]
        public string SubProductName { get; set; }

        [Display(Name = "Descripcion del producto")]
        public string Description { get; set; }
        [Display(Name = "Estiba")]
        public Int64 ControlPalletsId { get; set; }
        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }
        [Display(Name = "Sacos")]
        public int QuantitySacos { get; set; }

        [Display(Name = "Precio")]
        public double Price { get; set; }
        [Display(Name = "Total")]
        public double Total { get; set; }
        [Display(Name = "Bodega")]
        public Int64 WareHouseId { get; set; }
        [Display(Name = "Centro de costos")]
        public Int64 CostCenterId { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
