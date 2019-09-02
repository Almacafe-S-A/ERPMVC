using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class JournalEntry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 JournalEntryId { get; set; }

        public JournalEntry()
        {
            JournalEntryLines = new HashSet<JournalEntryLine>();
        }

        [Display(Name = "Id Libro Mayor")]
        public int? GeneralLedgerHeaderId { get; set; }
        public int? PartyId { get; set; }
        [Display(Name = "Tipos de Voucher")]
        public Int32 VoucherType { get; set; }
        [Display(Name = "Nombre de Tipo de Voucher")]
        public string TypeJournalName { get; set; }

        [Display(Name = "Fecha de creacion")]
        public DateTime Date { get; set; }
        [Display(Name = "Fecha de Posteo")]
        public DateTime DatePosted { get; set; }
        public string Memo { get; set; }
        public string ReferenceNo { get; set; }
        public bool? Posted { get; set; }
        //public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }
        //public virtual int Party { get; set; }
        [Display(Name = "Id de Registro Pago")]
        public Int64 IdPaymentCode { get; set; }
        public Int64 IdTypeofPayment { get; set; }
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; }
        [Required]
        [Display(Name = "Usuario de creacion")]
        public string CreatedUser { get; set; }
        [Required]
        [Display(Name = "Usuario de modificacion")]
        public string ModifiedUser { get; set; }
        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Display(Name = "Fecha de Modificacion")]
        public DateTime ModifiedDate { get; set; }

    }
}
