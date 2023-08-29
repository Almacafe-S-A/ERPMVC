using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CreditNote
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CreditNoteId { get; set; }
        public string CreditNoteName { get; set; }
        [Display(Name = "Envío")]
        public int ShipmentId { get; set; }

        [Display(Name = "Fiscal")]
        public bool Fiscal { get; set; }

        [Display(Name = "Fecha de nota de crédito")]
        public DateTime CreditNoteDate { get; set; }

        public int CreditNoteTypeId { get; set; }

        [Display(Name = "Factura Asociada")]
        public Int64? InvoiceId { get; set; }


        [Display(Name = "Certificado depósito")]
        public Int64 CertificadoDepositoId { get; set; }

        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; }



        [Display(Name = "Tipo de Nota de crédito")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Número de Nota de crédito")]
        public string NumeroDEI { get; set; }

        public string NumeroDEIDocumentoAsociado { get; set; }

        [Display(Name = "Número de inicio")]
        public string NoInicio { get; set; }

        [Display(Name = "Número fin")]
        public string NoFin { get; set; }

        public string RangoAutorizado { get; set; }

        [Display(Name = "Fecha Limite de emisión")]
        public DateTime FechaLimiteEmision { get; set; }

        [Display(Name = "Número de Factura")]
        public string CAI { get; set; }

        public Int64? JournalEntryId { get; set; }
        [ForeignKey("JournalEntryId")]
        public JournalEntry JournalEntry { get; set; }

        [Display(Name = "Número de orden de compra exenta")]
        public string NoOCExenta { get; set; }

        [Display(Name = "Número de constancia de registro de exoneracion")]
        public string NoConstanciadeRegistro { get; set; }

        [Display(Name = "Número de registro de la SAG")]
        public string NoSAG { get; set; }

        [Display(Name = "RTN")]
        public string RTN { get; set; }

        [Display(Name = "Telefono")]
        public string Tefono { get; set; }

        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Nombre Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Id")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Nombre Producto")]
        public string ProductName { get; set; }




        [Display(Name = "SubProducto")]
        public Int64? SubProductId { get; set; }

        [Display(Name = "Nombre SubProducto")]
        public string SubProductName { get; set; }




        [Display(Name = "Tipo de ventas")]
        public int SalesTypeId { get; set; }

        [Display(Name = "Observación")]
        public string Remarks { get; set; }

        [Display(Name = "Monto")]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "Money")]
        public decimal SubTotal { get; set; }

        [Display(Name = "Descuento")]
        [Column(TypeName = "Money")]
        public decimal Discount { get; set; }

        [Column(TypeName = "Money")]
        [Display(Name = "Impuesto")]
        public decimal Tax { get; set; }




        [Display(Name = "Total exento")]
        [Column(TypeName = "Money")]
        public decimal TotalExento { get; set; }

        [Display(Name = "Total exonerado")]
        [Column(TypeName = "Money")]
        public decimal TotalExonerado { get; set; }

        [Display(Name = "Total Gravado")]
        [Column(TypeName = "Money")]
        public decimal TotalGravado { get; set; }

        [Column(TypeName = "Money")]
        public decimal Total { get; set; }

        public string TotalLetras { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        public string AprobadoPor { get; set; }

        public DateTime? AprobadoEl { get; set; }

        public string RevisadoPor { get; set; }

        public DateTime? RevisadoEl { get; set; }

        public string Impreso { get; set; }
        public List<CreditNoteLine> CreditNoteLine { get; set; } = new List<CreditNoteLine>();

    }
}
