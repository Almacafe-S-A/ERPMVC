using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Kardex
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 KardexId { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Fecha")]
        public DateTime KardexDate { get; set; }

        [Display(Name = "Tipo de documento")]
        public Int64 DocType { get; set; }

        //[Display(Name = "Tipo de documento")]
        //public string DocName { get; set; }

        [Display(Name = "Documento")]
        public Int64? DocumentId { get; set; }

        [Display(Name = "Documento")]
        public string DocumentName { get; set; }

        [Display(Name = "Fecha de Kardex")]
        public DateTime DocumentDate { get; set; }


        [Display(Name = "Entrada/Salida")]
        public Int32? TypeOperationId { get; set; }

        [Display(Name = "Entrada/Salida")]
        public string TypeOperationName { get; set; }

        [Display(Name = "Moneda")]
        public Int64? CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }


        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }


        [Display(Name = "Sucursal")]
        public Int64? BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Bodega")]
        public Int64? WareHouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WareHouseName { get; set; }

        //[Display(Name = "Recibo")]
        //public Int64? GoodsReceivedId { get; set; }

        [Display(Name = "Estiba")]
        public Int64? ControlEstibaId { get; set; }

        [Display(Name = "Estiba")]
        public string ControlEstibaName { get; set; }

        [Display(Name = "Producto")]
        public Int64? ProducId { get; set; }

        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Producto")]
        public Int64? SubProducId { get; set; }

        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64? UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Saldo Anterior")]
        public double? SaldoAnterior { get; set; }


        [Display(Name = "Entrada")]
        public double? QuantityEntry { get; set; }

        [Display(Name = "Salida")]
        public double? QuantityOut { get; set; }

        [Display(Name = "Entrada de sacos")]
        public double? QuantityEntryBags { get; set; }

        [Display(Name = "Salida de sacos")]
        public double? QuantityOutBags { get; set; }

        [Display(Name = "Entrada certificado deposito")]
        public double? QuantityEntryCD { get; set; }

        [Display(Name = "Salida de certificado de deposito")]
        public double? QuantityOutCD { get; set; }

        [Display(Name = "Saldo Certificado depositos")]
        public double? TotalCD { get; set; }

        [Display(Name = "Saldo sacos")]
        public double? TotalBags { get; set; }

        [Display(Name = "Saldo")]
        public double? Total { get; set; }


        [Display(Name = "Centro de costos")]
        public Int64? CostCenterId { get; set; }

        [Display(Name = "Centro de costos")]
        public string CostCenterName { get; set; }

        //[Display(Name = "Cantidad minima en existencia por producto")]
        //public double? MinimumExistance { get; set; }


    }

    public class KardexParam
    {
        public Int64 DocumentId { get; set; }


        public string DocumentName { get; set; }

        public Int64 SubProducId { get; set; }
    }

}
