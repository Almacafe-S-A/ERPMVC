using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPMVC.Models
{
    public class InvoicePaymentsLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InvoicePaymentId { get; set; }

        [ForeignKey("InvoicePaymentId")]
        public InvoicePayments InvoicePayment { get; set; }

        public Int64? SubProductId { get; set; }
        [ForeignKey("SubProductId")]
        [UIHint("ServicioBodega")]
        public SubProduct SubProduct { get; set; }

        public int? InvoivceId { get; set; }

        [ForeignKey("InvoivceId")]
        public Invoice Invoice { get; set; }

        public string NoDocumento { get; set; }

        public int TipoDocumento { get; set; }

        public string SubProductName { get; set; }

        public decimal ValorOriginal { get; set; }

        public decimal MontoAdeudaPrevio { get; set; }
        public decimal MontoPagado { get; set; }

        public decimal MontoRestante { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public DateTime FechaCreacion { get; set; }









    }
}
