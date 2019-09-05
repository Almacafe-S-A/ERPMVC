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

        [Display(Name = "Id Clase Cuenta")]
        public int AccountId { get; set; }
        [Display(Name = "Tipo de Movimiento")]
        public int DrCr { get; set; }
        [Display(Name = "Monto Movimiento")]
        public double Amount { get; set; }
        public string Memo { get; set; }
        public virtual JournalEntry JournalEntry { get; set; }
        public virtual Account Account { get; set; }
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
