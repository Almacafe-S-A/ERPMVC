﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GoodsDeliveryAuthorizationLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Linea")]
        public Int64 GoodsDeliveryAuthorizationLineId { get; set; }

        public Int64 CertificadoLineId { get; set; }
        [Display(Name = "Autorizacion Id")]
        public Int64 GoodsDeliveryAuthorizationId { get; set; }
        [Display(Name = "Número de certificado")]
        [Required]
        public Int64 NoCertificadoDeposito { get; set; }

        public int? Pda { get; set; }

        [Required]
        [Display(Name = "Producto cliente")]
        public Int64 SubProductId { get; set; }
        [Display(Name = "Producto cliente")]
        public string SubProductName { get; set; }
        [ForeignKey("ProductoId")]
        [UIHint("ProductoCliente")]
        public CustomerProduct Product { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }
        [UIHint("UOM")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [Display(Name = "Bodega")]
        public Int64 WarehouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WarehouseName { get; set; }

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        public decimal Saldo { get; set; }

        [Display(Name = "Precio")]
        public double Price { get; set; }

        [Display(Name = "Valor del certificado")]
        public double valorcertificado { get; set; }

        public decimal? DerechosFiscales { get; set; }

        public decimal? ValorUnitarioDerechos { get; set; }

        [Display(Name = "Valor a pagar impuestos")]
        public double ValorImpuestos { get; set; }

        [Display(Name = "Saldo disponible")]
        public double SaldoProducto { get; set; }
    }



    public class GoodsDeliveryAuthorizationLineDTO : GoodsDeliveryAuthorizationLine
    {
        public double ValorImpuestosOriginal { get; set; }
        public double QuantityOriginal { get; set; }
    }
}
