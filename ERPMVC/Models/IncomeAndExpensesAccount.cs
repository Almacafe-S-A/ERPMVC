using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class IncomeAndExpensesAccount
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IncomeAndExpensesAccountId { get; set; }

        [Display(Name = "Banco")]
        public Int64 BankId { get; set; }

        [Display(Name = "Banco")]
        public string BankName { get; set; }

        [Display(Name = "Tipo de cuenta")]
        public Int64 TypeAccountId { get; set; }
        [Display(Name = "Tipo de cuenta")]
        public string TypeAccountName { get; set; }

        [Display(Name = "Descripción de la cuenta")]
        public string AccountDescription { get; set; }

        [Display(Name = "Estado")]
        public Int64 EstadoId { get; set; }

        [Display(Name = "Estado")]
        public string EstadoName { get; set; }

        public string UsuarioEjecucion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }



        List<IncomeAndExpenseAccountLine> IncomeAndExpenseAccountLine = new List<IncomeAndExpenseAccountLine>();

    }
}
