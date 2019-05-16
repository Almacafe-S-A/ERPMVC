using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ControlPallets
    {
        [Display(Name = "Estiba Id")]
        public Int64 ControlPalletsId { get; set; }
        public string Motorista { get; set; }
        public int WarehouseId { get; set; }
        public DateTime DocumentDate { get; set; }
        public Int64 ProductId { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        public string DescriptionProduct { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public int PalletId { get; set; }
        public int EsIngreso { get; set; }
        public int EsSalida { get; set; }
        public int SubTotal { get; set; }
        public int TotalSacos { get; set; }
        public int SacosDevueltos { get; set; }
        public double QQPesoBruto { get; set; }
        public double Tara { get; set; }
        public double QQPesoNeto { get; set; }
        public double QQPesoFinal { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }
}
