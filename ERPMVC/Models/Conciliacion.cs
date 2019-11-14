using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Conciliacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id de Conciliacion")]
        public int ConciliacionId { get; set; }

        [ForeignKey("IdBanco")]
        [Display(Name = "Id de Banco")]
        public Int64 BankId { get; set; }

        [ForeignKey("AccountId")]
        [Display(Name = "Numero de cuenta Contable")]
        public Int64 AccountId { get; set; }

        [Required]
        [Display(Name = "Nombre del Banco")]
        public string BankName { get; set; }
        [Display(Name = "Cuenta Bancaria")]
        public Int64 CheckAccountId { get; set; }
        [Display(Name = "Fecha Inicio")]
        public DateTime DateBeginReconciled { get; set; }
        [Display(Name = "Fecha Fin")]
        public DateTime DateEndReconciled { get; set; }

        [Required]
        [Display(Name = "Fecha de Conciliacion")]
        public DateTime FechaConciliacion { get; set; }

        [Required]
        [Display(Name = "Saldo Conciliado")]
        public Double SaldoConciliado { get; set; }

        

        [Required]
        [Display(Name = "FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Required]
        [Display(Name = "UsuarioCreacion")]
        public string UsuarioCreacion { get; set; }

        [Required]
        [Display(Name = "UsuarioModificacion")]
        public string UsuarioModificacion { get; set; }

        public List<ConciliacionLinea> ConciliacionLinea { get; set; }

    }
}
