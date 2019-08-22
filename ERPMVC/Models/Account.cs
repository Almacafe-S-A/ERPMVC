using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 AccountId { get; set; }
        [Display(Name = "Id Jerarquia Contable")]
        public int? ParentAccountId { get; set; }
        [Display(Name = "Id de la Empresa")]
        public Int64 CompanyInfoId { get; set; }
        [Required]
        [Display(Name = "Codigo Contable")]
        [StringLength(50)]
        public string AccountCode { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Nombre de la cuenta")]
        public string AccountName { get; set; }
        [StringLength(200)]
        [Display(Name = "Descripcion de la cuenta")]
        public string Description { get; set; }
        [Display(Name = "Activa:")]
        public bool IsCash { get; set; }
        [Display(Name = "Contracuenta:")]
        public bool IsContraAccount { get; set; }
        [Display(Name = "Nivel Contable")]
        public Int64 HierarchyAccount { get; set; }
        [Display(Name = "Bloqueo para Diarios:")]
        public bool BlockedInJournal { get; set; }
        [Display(Name = "Id")]
        public Int64 TypeAccountId { get; set; }

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
        [Display(Name = "Fecha de Modificacion")]
        public DateTime FechaModificacion { get; set; }
        [Display(Name = "Saldo Contable")]
        public double AccountBalance { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] RowVersion { get; set; }
        public virtual Account ParentAccount { get; set; }

        public virtual AccountClass AccountClass { get; set; }

        public virtual CompanyInfo Company { get; set; }

        public virtual ICollection<Account> ChildAccounts { get; set; }

        [NotMapped]
        public virtual ICollection<MainContraAccount> ContraAccounts { get; set; }
        public virtual ICollection<GeneralLedgerLine> GeneralLedgerLines { get; set; }

        public decimal GetBalance()
        {
            decimal balance = 0;

            var dr = from d in GeneralLedgerLines
                     where d.DrCr == 1
                     select d;

            var cr = from c in GeneralLedgerLines
                     where c.DrCr == 2
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
    }

}
