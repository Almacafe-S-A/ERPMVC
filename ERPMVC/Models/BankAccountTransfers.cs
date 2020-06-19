using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class BankAccountTransfers
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public string SourceBank { get; set; }

        public string SourceCurrency { get; set; }

        public string TargetBank { get; set; }

        public string TargetCurrency { get; set; }

        public Int64 SourceAccountManagementId { get; set; }
        [ForeignKey("SourceAccountManagementId")]
        public AccountManagement SourceAccountManagement { get; set; }

        public Int64 DestinationAccountManagementId { get; set; }
        [ForeignKey("DestinationAccountManagementId")]
        public AccountManagement DestinationAccountManagement { get; set; }
        [Column(TypeName = "Money")]
        public double SourceAmount { get; set; }
        [Column(TypeName = "Money")]
        public double DestinationAmount { get; set; }

        public Int64? ExchangeRateId { get; set; }
        [ForeignKey("ExchangeRateId")]
        public ExchangeRate ExchangeRate { get; set; }

        [Column(TypeName = "Money")]
        public double Rate { get; set; }

        public string Notes { get; set; }

        public Int64 JournalEntryId { get; set; }

        public JournalEntry JournalEntry { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }
    }
}
