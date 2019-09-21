using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class AccountingChilds
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 AccountingChildsId { get; set; }

        [Display(Name = "Id Jerarquia Contable")]
        public Int64 ParentAccountId { get; set; }
        [Display(Name = "Id de la Empresa")]
        public Int64 CompanyInfoId { get; set; }



        [MaxLength(5000)]
        [Display(Name = "Descripcion de la cuenta")]
        public string Description { get; set; }
        [Display(Name = "Estado")]
        public bool IsCash { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public int AccountClasses { get; set; }
        [Display(Name = "Contracuenta:")]
        public bool IsContraAccount { get; set; }
        [Display(Name = "Id")]
        public Int64 TypeAccountId { get; set; }
        [Display(Name = "Bloqueo para Diarios:")]
        public bool BlockedInJournal { get; set; }
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
        public string CreatedUser { get; set; }
        [Required]
        [Display(Name = "Usuario de modificacion")]
        public string ModifiedUser { get; set; }
        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Display(Name = "Fecha de modificacion")]
        public DateTime ModifiedDate { get; set; }
        public virtual Accounting Accounting { get; set; }
    }
}
