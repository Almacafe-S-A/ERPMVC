using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ProformaInvoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Número de Proforma")]
        public int ProformaId { get; set; }
        [Display(Name = "Nombre Cliente")]
        public string ProformaName { get; set; }

        [Display(Name = "RTN")]
        public string RTN { get; set; }

        [Display(Name = "Teléfono")]
        public string Tefono { get; set; }
       
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Sucursal Nombre")]
        public string BranchName { get; set; }

        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Area Utilizada")]
        public Int64 CustomerAreaId { get; set; }


        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Nombre Producto")]
        public string ProductName { get; set; }


        [Display(Name = "Fecha de inicio")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha de fin")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Fecha proforma")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Fecha de entrega")]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        [Display(Name = "Moneda tasa")]
        public double Currency { get; set; }

        [Display(Name = "Número de referencia de cliente")]
        public string CustomerRefNumber { get; set; }


        [Display(Name = "Cotización Asociada")]
        public Int64 SalesOrderId { get; set; }

        [Display(Name = "Certificado depósito")]
        public Int64 CertificadoDepositoId { get; set; }

        [Display(Name = "Tipo de ventas")]
        public int SalesTypeId { get; set; }
        public string Remarks { get; set; }

        [Display(Name = "Monto")]
        public double Amount { get; set; }
        public double SubTotal { get; set; }

        [Display(Name = "Descuento")]
        public double Discount { get; set; }

        [Display(Name = "Impuesto%")]
        public double Tax { get; set; }

        [Display(Name = "Impuesto 18%")]
        public double Tax18 { get; set; }

        [Display(Name = "Flete")]
        public double Freight { get; set; }

        [Display(Name = "Total exento")]
        public double TotalExento { get; set; }

        [Display(Name = "Total exonerado")]
        public double TotalExonerado { get; set; }

        [Display(Name = "Total Gravado")]
        public double TotalGravado { get; set; }

        [Display(Name = "Total Gravado 18%")]
        public double TotalGravado18 { get; set; }
        public double Total { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        public string Impreso { get; set; }

        public List<ProformaInvoiceLine> ProformaInvoiceLine { get; set; } = new List<ProformaInvoiceLine>();
    }
}
