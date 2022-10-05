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
        [Display(Name = "Tipo Beneficiario")]
        public int PartyTypeId { get; set; }

        [Display(Name = "Tipo Beneficiario")]
        public string PartyTypeName { get; set; }

        [Display(Name = "Id Documento")]
        public Int64 DocumentId { get; set; }
        [Display(Name = "Beneficiario")]
        public int? PartyId { get; set; }
        [Display(Name = "Nombre Beneficiario")]
        public string PartyName { get; set; }
        [Display(Name = "Tipos de Documento")]
        public Int32? VoucherType { get; set; }
        [Display(Name = "Nombre de Tipo de Voucher")]
        public string TypeJournalName { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime Date { get; set; }
        [Display(Name = "Fecha de Transacción")]
        public DateTime DatePosted { get; set; }
        [Display(Name = "Sinopsis")]
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

        [Display(Name = "Total Débito")]
        public double TotalDebit { get; set; }

        [Display(Name = "Total Crédito")]
        public double TotalCredit { get; set; }

        [Display(Name = "Tipo de Asiento")]
        public Int32 TypeOfAdjustmentId { get; set; }

        [Display(Name = "Tipo de Asiento")]
        public string TypeOfAdjustmentName { get; set; }
        [Display(Name = "Estado")]
        public Int64? EstadoId { get; set; }
        [Display(Name = "Estado")]
        public string EstadoName { get; set; }
        [Display(Name = "Aprobado Por")]
        public string ApprovedBy { get; set; }
        [Display(Name = "Fecha de aprobación")]
        public DateTime? ApprovedDate { get; set; }

        public string Periodo { get; set; }

        public int? PeriodoId { get; set; }
        [ForeignKey("PeriodoId")]
        public Periodo PeriodoNav { get; set; }


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
