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
        [Display(Name = "Id")]
        public int ConciliacionId { get; set; }

        public Int64 BankId { get; set; }

        [Display(Name = "Id")]
        public Int64 AccountId { get; set; }

        [Required]
        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Display(Name = "Cuenta Bancaria")]
        public Int64 CheckAccountId { get; set; }

        [Required]
        [Display(Name = "FechaConciliacion")]
        public DateTime FechaConciliacion { get; set; }

        [Display(Name = "Fecha Inicio")]
        public DateTime DateBeginReconciled { get; set; }
        [Display(Name = "Fecha Fin")]
        public DateTime DateEndReconciled { get; set; }

        [Required]
        [Display(Name = "SaldoConciliado")]
        public Double SaldoConciliado { get; set; }


        public string Estado { get; set; }
        public Int64 EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estados Estados { get; set; }


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

        [Required]
        [Display(Name = "Saldo en Estado de Cuenta de Banco")]
        public decimal SaldoBanco { get; set; }

        [Required]
        [Display(Name = "Saldo en Libro Mayor de Banco")]
        public decimal SaldoLibro { get; set; }

        public List<ConciliacionLinea> ConciliacionLinea { get; set; }

    }
}
