using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EndososCertificados
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 EndososCertificadosId { get; set; }

        [Display(Name = "Id certificado")]
        public Int64 IdCD { get; set; }

        [Display(Name = "Número de certificado")]
        public Int64 NoCD { get; set; }

        [Display(Name = "Fecha de documento")]
        public DateTime DocumentDate { get; set; }


        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Banco")]
        public Int64 BankId { get; set; }

        [Display(Name = "Banco")]
        public string BankName { get; set; }

        [Display(Name = "Cantidad a endosar")]
        public double CantidadEndosar { get; set; }

        [Display(Name = "Moneda")]
        public Int64 CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        [Display(Name = "Valor a endosar")]
        public double ValorEndosar { get; set; }

        [Display(Name = "Tipo de endoso")]
        public Int64 TipoEndosoId { get; set; }

        [Display(Name = "Tipo de endoso")]
        public string TipoEndosoName { get; set; }


        [Display(Name = "Firmado en")]
        public string FirmadoEn { get; set; }


        [Display(Name = "Tipo de endoso")]
        public Int64 TipoEndoso { get; set; }

        [Display(Name = "Nombre de endoso")]
        public string NombreEndoso { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Fecha otorgada")]
        public DateTime FechaOtorgado { get; set; }

        [Display(Name = "Tasa de interes")]
        public double TasaDeInteres { get; set; }

        [Display(Name = "Total endoso")]
        public double TotalEndoso { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public string Impreso { get; set; }

        public List<EndososCertificadosLine> EndososCertificadosLine { get; set; } = new List<EndososCertificadosLine>();

    }
}
