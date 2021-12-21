using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GoodsDeliveryAuthorization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 GoodsDeliveryAuthorizationId { get; set; }

        [Display(Name = "Nombre de autorizacion")]
        public string AuthorizationName { get; set; }
        [Display(Name = "Fecha de documento")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Fecha de autorización")]
        public DateTime AuthorizationDate { get; set; }

        [Display(Name = "Número de certificado de depósito")]
        public Int64 NoCD { get; set; }


        [Display(Name = "Comentarios")]
        public string Comments { get; set; }

        [Display(Name = "Descripción")]
        public string Name { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Moneda")]
        public Int64 CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        public string Certificados { get; set; }

        [Display(Name = "Banco")]
        public Int64? BankId { get; set; }
        [Display(Name = "Banco")]
        public string BankName { get; set; }
        [Display(Name = "Servicio")]
        public Int64 ProductId { get; set; }
        [Display(Name = "Servicio")]
        public string ProductName { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }
        [Display(Name = "Bodega")]
        public Int64 WareHouseId { get; set; }
        [Display(Name = "Bodega")]
        public string WareHouseName { get; set; }
        public double TotalCertificado { get; set; }
        public double TotalFinanciado { get; set; }

        [Display(Name = "Derechos Lps.")]
        public double DerechoLps { get; set; }

        [Display(Name = "Póliza No.")]
        public Int64 NoPoliza { get; set; }

        [Display(Name = "Delegado Fiscal")]
        public string DelegadoFiscal { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de Creacion")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public string UsuarioModificacion { get; set; }

        public List<GoodsDeliveryAuthorizationLine> GoodsDeliveryAuthorizationLine { get; set; } = new List<GoodsDeliveryAuthorizationLine>();
    }
}
