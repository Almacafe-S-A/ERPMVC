using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class VendorInvoice
    {
        [Display(Name = "Id")]
        public int VendorInvoiceId { get; set; }
        [Display(Name = "Nombre")]
        public string VendorInvoiceName { get; set; }
        [Display(Name = "Envío")]
        public int ShipmentId { get; set; }
        [Display(Name = "Orden de Compra")]
        public int? PurchaseOrderId { get; set; }
        [Display(Name = "Fecha de Factura")]
        public DateTime VendorInvoiceDate { get; set; }
        [Display(Name = "Fecha de vencimiento")]
        public DateTime VendorInvoiceDueDate { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Tipo de Factura")]
        public int VendorInvoiceTypeId { get; set; }

     
        //[Display(Name = "Sucursal")]
        //public string Sucursal { get; set; }


        [Display(Name = "Número de Factura")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Número de Factura")]
        public int NumeroDEI { get; set; }

        [Display(Name = "Número de Inicio")]
        public string NoInicio { get; set; }

        [Display(Name = "Número Fin")]
        public string NoFin { get; set; }

        [Display(Name = "Fecha Límite")]
        public DateTime FechaLimiteEmision { get; set; }

        [Display(Name = "Número de Factura")]
        public string CAI { get; set; }

        [Display(Name = "Número de orden de compra exenta")]
        public string NoOCExenta { get; set; }

        [Display(Name = "Número de constancia de registro de exoneración")]
        public string NoConstanciadeRegistro { get; set; }

        [Display(Name = "Número de registro de la SAG")]
        public string NoSAG { get; set; }

        [Display(Name = "RTN")]
        public string RTN { get; set; }
        [Display(Name = "Teléfono")]

        //public Int64 CostCenterId { get; set; }
        //[ForeignKey("CostCenterId")]
        //public CostCenter CostCenter { get; set; }
        public string Tefono { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Proveedor")]
        public Int64 VendorId { get; set; }

        [Display(Name = "Proveedor")]
        public string VendorName { get; set; }
        

        [Display(Name = "Fecha Factura")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Fecha Recibido")]
        public DateTime ReceivedDate { get; set; }

        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        [Display(Name = "Moneda Tasa")]
        public double Currency { get; set; }

        [Display(Name = "Número de referencia del Proveedor")]
        public string VendorRefNumber { get; set; }
        [Display(Name = "Tipo de ventas")]
        public int SalesTypeId { get; set; }

        [Display(Name = "Observación")]
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

        [Display(Name = "Total Exento")]
        public double TotalExento { get; set; }

        [Display(Name = "Total Exonerado")]
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

        public string Impreso { get; set; }
        public Int64 AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Accounting Account { get; set; }

        public List<VendorInvoiceLine> VendorInvoiceLine { get; set; } = new List<VendorInvoiceLine>();

    }
}
