using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class PhoneLines
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Línea Telefónica")]
        public Int64 PhoneLineId { get; set; }

        [Display(Name = "Id de Empleado")]
        public long IdEmpleado { get; set; }
        [Display(Name = "Nombre de Empleado")]
        public string NombreEmpleado { get; set; }
        [Display(Name = "Sucursal")]
        public int IdBranch { get; set; }
        [ForeignKey("IdBranch")]
        public Branch Branch { get; set; }

        [Display(Name = "Teléfono de la Empresa")]
        public string CompanyPhone { get; set; }

        [Display(Name = "Vencido Lps")]
        public double DueBalanceLps { get; set; }

        [Display(Name = "Vencido US")]
        public double DueBalanceUS { get; set; }

        [Display(Name = "Abono Lps")]
        public double PaymentLps { get; set; }

        [Display(Name = "Abono US")]
        public double PaymentUS { get; set; }

        [Display(Name = "Total Lps")]
        public double TotalLps { get; set; }

        [Display(Name = "Total US")]
        public double TotalUS { get; set; }
        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
