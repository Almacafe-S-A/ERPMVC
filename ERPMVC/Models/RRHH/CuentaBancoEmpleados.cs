using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CuentaBancoEmpleados
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Display(Name = "Id Banco")]
        public long BankId { get; set; }
        [Display(Name = "Banco")]
        public string BankName { get; set; }
        [Display(Name = "Numero de Cuenta")]
        public string NumeroCuenta { get; set; }
        [Display(Name = "Código Empleado")]
        public long IdEmpleado { get; set; }
        [Display(Name = "Nombre del Empleado")]
        public string NombreEmpleado { get; set; }

        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
