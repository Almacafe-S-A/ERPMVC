using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class IncomeAndExpenseAccountLine
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IncomeAndExpenseAccountLineId { get; set; }
        [Display(Name = "Ingresos y gastos")]
        public Int64 IncomeAndExpensesAccountId { get; set; }
        [Display(Name = "Tipo de documento")]
        public Int64 TypeDocument { get; set; }
        [Display(Name = "Documento")]
        public Int64 DocumentId { get; set; }
        [Display(Name = "Monto")]
        public double Amount { get; set; }
        [Display(Name = "Fecha de documento")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Débito/Crédito")]
        public string DebitCredit { get; set; }
        
    }

}
