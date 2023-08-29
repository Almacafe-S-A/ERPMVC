using ERPMVC.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPMVC.Models
{
    public class InvoicePayments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NoPago { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        public DateTime FechaPago { get; set; }

        public int? InvoivceId { get; set; }

        [ForeignKey("InvoivceId")]
        public Invoice Invoice { get; set; }

        public double MontoAdeudaPrevio { get; set; }
        public double MontoPagado { get; set; }

        public double MontoAdeudado { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Depositante { get; set; }

        public string Referrencia { get; set; }

        public string Observaciones { get; set; }


        public string NoDocumentos { get; set; }

        public string Sinopsis { get; set; }

        public Int64? CuentaBancariaId { get; set; }
        [ForeignKey("CuentaBancariaId ")]
        public AccountManagement accountManagement { get; set; }

        public Int64 Bank { get; set; }

        public string BankName { get; set; }

        public int TipoPago { get; set; }

        public Int64? JournalId { get; set; }

        [ForeignKey("JournalId ")]
        public JournalEntry journalEntry { get; set; }

        public string CantidadenLetras { get; set; }

        public int EstadoId { get; set; }

        public string Estado { get; set; }


        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        [NotMapped]
        public Int64[] Facturas { get; set; }




        public List<InvoicePaymentsLine> InvoicePaymentsLines { get; set; }






    }



    public class InvoicePaymentsDTO {
        public List<Identificador> Facturas { get; set; }

        public int Id { get; set; }




    }
}
