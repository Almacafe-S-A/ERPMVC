using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Invoice
    {
        [Display(Name = "Id")]
        public int InvoiceId { get; set; }
        [Display(Name = "Nombre")]
        public string InvoiceName { get; set; }
        [Display(Name = "Envio")]
        public int ShipmentId { get; set; }
        [Display(Name = "Fecha de Factura")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Fecha de vencimiento")]
        public DateTime InvoiceDueDate { get; set; }
        [Display(Name = "Tipo de Factura")]
        public int InvoiceTypeId { get; set; }

        [Display(Name = "Cotización Asociada")]
        public Int64 SalesOrderId { get; set; }

        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; }

        [Display(Name = "Sucursal")]
        public string Caja { get; set; }

        [Display(Name = "Numero de Factura")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Numero de Factura")]
        public int NumeroDEI { get; set; }

        [Display(Name = "Numero de inicio")]
        public string NoInicio { get; set; }

        [Display(Name = "Numero fin")]
        public string NoFin { get; set; }

        [Display(Name = "Fecha Limite")]
        public DateTime FechaLimiteEmision { get; set; }

        [Display(Name = "Numero de Factura")]
        public string CAI { get; set; }

        [Display(Name = "Numero de orden de compra exenta")]
        public string NoOCExenta { get; set; }

        [Display(Name = "Numero de constancia de registro de exoneracion")]
        public string NoConstanciadeRegistro { get; set; }

        [Display(Name = "Numero de registro de la SAG")]
        public string NoSAG { get; set; }

        [Display(Name = "RTN")]
        public string RTN { get; set; }

        [Display(Name = "Telefono")]
        public string Tefono { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress]
        public string Correo { get; set; }

        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Fecha factura")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Fecha de entrega")]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Numero de referencia de cliente")]
        public string CustomerRefNumber { get; set; }
        [Display(Name = "Tipo de ventas")]
        public int SalesTypeId { get; set; }

        [Display(Name = "Observacion")]
        public string Remarks { get; set; }

        [Display(Name = "Monto")]
        public double Amount { get; set; }
        public double SubTotal { get; set; }

        [Display(Name = "Descuento")]
        public double Discount { get; set; }


        [Display(Name = "Impuesto")]
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

        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public List<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    }
}
