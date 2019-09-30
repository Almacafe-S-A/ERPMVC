using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Accounting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 AccountId { get; set; }

        [Display(Name = "Id Jerarquia Contable")]
        public int? ParentAccountId { get; set; }
        [Display(Name = "Id de la Empresa")]
        public Int64 CompanyInfoId { get; set; }

        [Display(Name = "Saldo Contable")]
        public double AccountBalance { get; set; }


        [MaxLength(5000)]
        [Display(Name = "Descripcion de la cuenta")]
        public string Description { get; set; }
        [Display(Name = "Mostrar Saldos")]
        public bool? IsCash { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public int AccountClasses { get; set; }
        [Display(Name = "Contracuenta:")]
        public bool IsContraAccount { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public Int64 TypeAccountId { get; set; }
        [Display(Name = "Bloqueo para Diarios:")]
        public bool? BlockedInJournal { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Codigo Contable")]
        public string AccountCode { get; set; }

        [Required]
        [Display(Name = "Nivel de Jerarquia:")]
        public Int64 HierarchyAccount { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Nombre de la cuenta")]
        public string AccountName { get; set; }

        [Required]
        [Display(Name = "Usuario de creacion")]
        public string UsuarioCreacion { get; set; }
        [Required]
        [Display(Name = "Usuario de modificacion")]
        public string UsuarioModificacion { get; set; }
        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [Display(Name = "Fecha de modificacion")]
        public DateTime FechaModificacion { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        [Display(Name = "Version Fila")]
        public byte[] RowVersion { get; set; }
        [Display(Name = "Padre de la cuenta")]
        public virtual Accounting ParentAccount { get; set; }

        public virtual AccountClass AccountClass { get; set; }

        public virtual CompanyInfo Company { get; set; }

        public virtual ICollection<Accounting> ChildAccounts { get; set; }
        public virtual List<AccountingChilds> AccountingChilds { get; set; }
        [NotMapped]
        public virtual ICollection<MainContraAccount> ContraAccounts { get; set; }
        public virtual ICollection<GeneralLedgerLine> GeneralLedgerLines { get; set; }


        /*private decimal GetDebitCreditBalance(DrOrCrSide side)
        {
            decimal balance = 0;

            if (side == DrOrCrSide.Dr)
            {
                var dr = from d in GeneralLedgerLines
                         where d.DrCr == DrOrCrSide.Dr
                         select d;

                balance = dr.Sum(d => d.Amount);
            }
            else
            {
                var cr = from d in GeneralLedgerLines
                         where d.DrCr == DrOrCrSide.Cr
                         select d;

                balance = cr.Sum(d => d.Amount);
            }

            return balance;
        }
        
        public decimal GetBalance()
        {
            decimal balance = 0;

            var dr = from d in GeneralLedgerLines
                     where d.DrCr == DrOrCrSide.Dr
                     select d;

            var cr = from c in GeneralLedgerLines
                     where c.DrCr == DrOrCrSide.Cr
                     select c;

            decimal drAmount = dr.Sum(d => d.Amount);
            decimal crAmount = cr.Sum(c => c.Amount);

            if (AccountClass.NormalBalance == "Dr")
            {
                balance = drAmount - crAmount;
            }
            else
            {
                balance = crAmount - drAmount;
            }

            return balance;
        }
        */
        public bool CanPost()
        {
            if (ChildAccounts != null && ChildAccounts.Count > 0)
                return false;
            return true;
        }
        /*
        /// <summary>
        /// Used to indicate the increase or decrease on account. When there is a change in an account, that change is indicated by either debiting or crediting that account.
        /// </summary>
        /// <param name="amount">The amount to enter on account.</param>
        /// <returns></returns>
        public DrOrCrSide DebitOrCredit(decimal amount)
        {
            var side = DrOrCrSide.Dr;

            if (this.AccountId == (int)AccountClasses.Assets || this.AccountId == (int)AccountClasses.Expense)
            {
                if (amount > 0)
                    side = DrOrCrSide.Dr;
                else
                    side = DrOrCrSide.Cr;
            }

            if (this.AccountId == (int)AccountClasses.Liabilities || this.AccountId == (int)AccountClasses.Equity || this.AccountId == (int)AccountClasses.Revenue)
            {
                if (amount < 0)
                    side = DrOrCrSide.Dr;
                else
                    side = DrOrCrSide.Cr;
            }

            if (this.IsContraAccount)
            {
                if (side == DrOrCrSide.Dr)
                    return DrOrCrSide.Cr;
                if (side == DrOrCrSide.Cr)
                    return DrOrCrSide.Dr;
            }

            return side;
        }*/

    }
}
