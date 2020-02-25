using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CierresAccounting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CierreAccountingId { get; set; }
        public Int64 AccountId { get; set; }
        [Display(Name = "Id Jerarquia Contable")]
        public int? ParentAccountId { get; set; }
        [Display(Name = "Id de la Empresa")]
        public Int64 CompanyInfoId { get; set; }

        [Display(Name = "Saldo Contable")]
        public double AccountBalance { get; set; }

        public int BitacoraCierreContableId { get; set; }
        [ForeignKey("BitacoraCierreContableId")]
        //public BitacoraCierreContable BitacoraCierreContable { get; set; }


        [MaxLength(5000)]
        [Display(Name = "Descripcion de la cuenta")]
        public string Description { get; set; }
        [Display(Name = "Mostar Saldos")]
        public bool IsCash { get; set; }


        [Display(Name = "Id")]
        public Int64 TypeAccountId { get; set; }
        [Display(Name = "Bloqueo para Diarios:")]
        public bool BlockedInJournal { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Codigo Contable")]
        public string AccountCode { get; set; }
        [Display(Name = "Id de estado")]
        public Int64? IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

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
        //public virtual Accounting ParentAccount { get; set; }

        public DateTime FechaCierre { get; set; }


        //public virtual CompanyInfo Company { get; set; }


    }


    //public class AccountingCierresDTO : Accounting
    //{
    //    public AccountingCierresDTO()
    //    {

    //    }

    //    public double Debit { get; set; }

    //    public double Credit { get; set; }

    //    public double TotalDebit { get; set; }

    //    public double TotalCredit { get; set; }

    //}

}
