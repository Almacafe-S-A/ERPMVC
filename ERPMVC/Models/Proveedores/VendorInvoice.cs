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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorInvoiceId { get; set; }
        public string VendorInvoiceName { get; set; }
        [Display(Name = "Envio")]
        public int ShipmentId { get; set; }


        [Display(Name = "Fecha de Factura")]
        public DateTime VendorInvoiceDate { get; set; }
        [Display(Name = "Fecha de vencimiento")]
        public DateTime VendorInvoiceDueDate { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime ExpirationDate { get; set; }
        [Display(Name = "Tipo de Factura")]
        public int VendorInvoiceTypeId { get; set; }


        [Display(Name = "Numero de Factura")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Numero de Factura")]
        public string NumeroDEI { get; set; }

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



        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Nombre Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Vendor")]
        public long VendorId { get; set; }

        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }


        [Display(Name = "RTN")]
        public string VendorRTN { get; set; }

        [Display(Name = "Nombre Proveedor")]
        public string VendorName { get; set; }


        public DateTime OrderDate { get; set; }

        public DateTime ReceivedDate { get; set; }




        [Display(Name = "Tipo de ventas")]
        public int SalesTypeId { get; set; }

        [Display(Name = "Observacion")]
        public string Remarks { get; set; }

        [Column(TypeName = "Money")]
        public double Tax { get; set; }

        [Column(TypeName = "Money")]
        public double TotalExento { get; set; }

        [Column(TypeName = "Money")]
        public double TotalGravado { get; set; }

        [Column(TypeName = "Money")]
        public double Total { get; set; }

        public string TotalLetras { get; set; }

        public Int64 IdEstado { get; set; }

        public string Estado { get; set; }

        public Int64? AccountIdCredito { get; set; }


        [ForeignKey("AccountIdCredito")]
        public Accounting Accountcredito { get; set; }

        public string AccountNameCredito { get; set; }

        public Int64? AccountIdGasto { get; set; }

        [ForeignKey("AccountIdGasto")]
        public Accounting AccountGasto { get; set; }

        public string AccountNameGasto { get; set; }


        public Int64? CostCenterId { get; set; }
        [ForeignKey("CostCenterId")]
        public CostCenter CostCenter { get; set; }

        public string CostCenterName { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public string Impreso { get; set; }

        public bool AplicaRetencion { get; set; }

        public bool RetecionPendiente { get; set; }

    }
}
