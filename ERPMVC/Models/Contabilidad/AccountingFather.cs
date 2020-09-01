using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class AccountingFather
    {
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
        public string DescriptionPadre { get; set; }
        [Display(Name = "Mostrar Saldos")]
        public bool? IsCashPadre { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public int AccountClasses { get; set; }
        [Display(Name = "Contracuenta:")]
        public bool IsContraAccount { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public Int64 TypeAccountIdPadre { get; set; }
        [Display(Name = "Bloqueo para Diarios:")]
        public bool? BlockedInJournalPadre { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Codigo Contable")]
        public string AccountCodePadre { get; set; }
        [Display(Name = "Id de estado")]
        public Int64 IdEstadoPadre { get; set; }
        [Display(Name = "Estado")]
        public string EstadoPadre { get; set; }
        [Required]
        [Display(Name = "Nivel de Jerarquia:")]
        public Int64 HierarchyAccount { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Nombre de la cuenta")]
        public string AccountNamePadre { get; set; }

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


        public virtual CompanyInfo Company { get; set; }

        public virtual ICollection<Accounting> ChildAccounts { get; set; }
        [NotMapped]
        public virtual ICollection<MainContraAccount> ContraAccounts { get; set; }
        public virtual ICollection<GeneralLedgerLine> GeneralLedgerLines { get; set; }

    }
}
