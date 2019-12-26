using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CheckAccount
    {
        [Display(Name = "Id")]
        public Int64 CheckAccountId { get; set; }

        [Display(Name = "Número de chequera")]
        public string CheckAccountNo { get; set; }

        [Display(Name = "Número de cuenta asociada")]
        public string AssociatedAccountNumber { get; set; }

        public Int64 AccountManagementId { get; set; }
        [ForeignKey("AccountManagementId")]
        public AccountManagement AccountManagement { get; set; }

        [Display(Name = "Banco")]
        public Int64 BankId { get; set; }

        [Display(Name = "Banco")]
        public string BankName { get; set; }

        [Display(Name = "Número Inicial")]
        public string NoInicial { get; set; }

        [Display(Name = "Número Final")]
        public string NoFinal { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Fecha de creacion")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificacion")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

    }


}
