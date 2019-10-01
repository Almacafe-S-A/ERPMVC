using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class JournalEntryLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id de linea Diario")]
        public Int64 JournalEntryLineId { get; set; }
        [Display(Name = "Id Entrada diario")]
        public Int64 JournalEntryId { get; set; }
        [StringLength(30)]
        [Display(Name = "Numero de Centro de Costo")]
        public string Num { get; set; }
        [StringLength(60)]
        [Display(Name = "Nombre de Centro Costo")]
        public string Description { get; set; }


        [Display(Name = "Cuenta")]
        public int AccountId { get; set; }

        [Display(Name = "Cuenta")]
        public string AccountName { get; set; }
        //  [Display(Name = "Tipo de Movimiento")]
        //   public int DrCr { get; set; }
        //[Display(Name = "Monto Movimiento")]
        //public double Amount { get; set; }

        [Display(Name = "Débito")]
        public double Debit { get; set; }
        [Display(Name = "Crédito")]
        public double Credit { get; set; }

        [Display(Name = "Débito moneda del sistema ")]
        public double DebitSy { get; set; }
        [Display(Name = "Crédito moneda del sistema")]
        public double CreditSy { get; set; }

        [Display(Name = "Débito moneda extranjera ")]
        public double DebitME { get; set; }
        [Display(Name = "Crédito moneda extranjera")]
        public double CreditME { get; set; }
        public string Memo { get; set; }
        public virtual JournalEntry JournalEntry { get; set; }
        public virtual Account Account { get; set; }
        [Required]
        [Display(Name = "Usuario de creación")]
        public string CreatedUser { get; set; }
        [Required]
        [Display(Name = "Usuario de modificación")]
        public string ModifiedUser { get; set; }
        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Display(Name = "Fecha de Modificación")]
        public DateTime ModifiedDate { get; set; }

    }
}
