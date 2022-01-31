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
        public Int64 GoodsDeliveryAuthorizationId { get; set; }

        [Display(Name = "Nombre de autorizacion")]
        public string AuthorizationName { get; set; }
        [Display(Name = "Fecha de documento")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Fecha de autorizacion")]
        public DateTime AuthorizationDate { get; set; }

        [Display(Name = "Fecha de documento")]
        public Int64 NoCD { get; set; }

        [Display(Name = "Fecha de documento")]
        public string Name { get; set; }

        public string Certificados { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        public string Autorizados { get; set; }



        [Display(Name = "Banco")]
        public Int64? BankId { get; set; }
        [Display(Name = "Banco")]
        public string BankName { get; set; }
        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }
        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Comentarios")]
        public string Comments { get; set; }

        public string URLCartaRetiro { get; set; }

        public string CartaRetiroDocName { get; set; }

        public string URLLiberacionEndoso { get; set; }

        public string LiberacionEndosoDocName { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de Creacion")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public string UsuarioModificacion { get; set; }

        public string Impreso { get; set; }

        [Display(Name = "Póliza No.")]
        public Int64 NoPoliza { get; set; }

        [Display(Name = "Delegado Fiscal")]
        public string DelegadoFiscal { get; set; }

        [Display(Name = "Seguro")]
        public string EmpresaSeguro { get; set; }


        [Display(Name = "Número de traslado")]
        public string NoTraslado { get; set; }


        [Display(Name = "Aduana")]
        public string Aduana { get; set; }

        [Display(Name = "Carta de porte o manifiesto No.")]
        public string ManifiestoNo { get; set; }

        public List<GoodsDeliveryAuthorizationLine> GoodsDeliveryAuthorizationLine { get; set; } = new List<GoodsDeliveryAuthorizationLine>();
    }
}
