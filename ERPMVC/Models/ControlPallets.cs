using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ControlPallets
    {
        [Display(Name = "Control Ingresos Id")]
        public Int64 ControlPalletsId { get; set; }
        public string Motorista { get; set; }      

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Bodega")]
        public int WarehouseId { get; set; }
        [Display(Name = "Fecha control de estiba")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Producto Cliente")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Producto Cliente")]
        public string SubProductName { get; set; }

        [Display(Name = "Servicio")]
        public Int64 ProductId { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Nombre de Cliente")]
        public string CustomerName { get; set; }
        [Display(Name = "Descripción de servicio")]
        public string DescriptionProduct { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Estiba No.")]
        public int PalletId { get; set; }        
        [Display(Name = "Es Ingreso")]
        public int EsIngreso { get; set; }
        [Display(Name = "Es Salida")]
        public int EsSalida { get; set; }
        public int SubTotal { get; set; }

        [Display(Name = "Total Sacos")]
        public int TotalSacos { get; set; }

        [Display(Name = "Total Sacos Yute")]
        public int TotalSacosYute { get; set; }

        [Display(Name = "Total sacos polietileno")]
        public int TotalSacosPolietileno { get; set; }

        [Display(Name = "Sacos devueltos")]
        public int SacosDevueltos { get; set; }

        [Display(Name = "QQ Peso Bruto")]
        public double QQPesoBruto { get; set; }
        public double Tara { get; set; }

        [Display(Name = "QQ Peso Neto")]
        public double QQPesoNeto { get; set; }

        [Display(Name = "QQ Peso Final")]
        public double QQPesoFinal { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public string Impreso { get; set; }

        [Display(Name = "Boleta de peso")]
        public Int64 WeightBallot { get; set; }

        [Display(Name = "Id Autorización")]
        public Int64 GoodsDeliveryAuthorizationId { get; set; }

        public List<ControlPalletsLine> _ControlPalletsLine = new List<ControlPalletsLine>();

    }
}
