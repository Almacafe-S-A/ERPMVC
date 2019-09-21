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
        [Display(Name = "Id Asiento Contable")]
        public Int64 JournalEntryId { get; set; }
       
        public JournalEntry()
        {
            JournalEntryLines = new HashSet<JournalEntryLine>();
        }

        [Display(Name = "Id Libro Mayor")]
        public int? GeneralLedgerHeaderId { get; set; }
        [Display(Name = "Socio de negocio")]
        public int? PartyId { get; set; }
        [Display(Name = "Tipos de Voucher")]
        public Int32 VoucherType { get; set; }
        [Display(Name = "Nombre de Tipo de Voucher")]
        public string TypeJournalName { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime Date { get; set; }
        [Display(Name = "Fecha de Posteo")]
        public DateTime DatePosted { get; set; }
        [Display(Name = "Memo")]
        public string Memo { get; set; }
        [Display(Name = "Número de referencia")]
        public string ReferenceNo { get; set; }
        [Display(Name = "Estado")]
        public bool? Posted { get; set; }
        [Display(Name = "Libro contable")]
        public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }
      //  public virtual int Party { get; set; }
        [Display(Name = "Id de Registro Pago")]
        public Int32 IdPaymentCode { get; set; }
        [Display(Name = "Tipo de Pago")]
        public Int32 IdTypeofPayment { get; set; }
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
