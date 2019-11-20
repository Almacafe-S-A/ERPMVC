using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPMVC.Models
{
    public class AccountManagement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 AccountManagementId { get; set; }

        [Required]
        [Display(Name = "Fecha de apertura")]
        public DateTime OpeningDate { get; set; }
        [Required]
        [Display(Name = "Numero de cuenta")]
        public string AccountType { get; set; }

        [Display(Name = "Numero de cuenta")]
        public string AccountNumber { get; set; }
        [Required]
        public Int64 BankId { get; set; }

        [Display(Name = "Institucion Financiera")]
        public string BankName { get; set; }
        [Required]
        public int CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        [Display(Name = "Descripcion")]
        public string Description { get; set; }

        public string Status { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }
    }
}
