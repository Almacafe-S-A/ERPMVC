using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EmployeeSalary
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 EmployeeSalaryId { get; set; }
        public long IdEmpleado { get; set; }
        [ForeignKey("IdEmpleado")]

        [Required]
        [Display(Name = "Salario")]
        public decimal? QtySalary { get; set; }
        [Display(Name = "Id de Frecuencia")]
        public Int64 IdFrequency { get; set; }
        [Display(Name = "Dia de Aplicacición")]
        public DateTime DayApplication { get; set; }

        [Display(Name = "Id de estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required]
        [Display(Name = "Usuario que lo crea")]
        public string CreatedUser { get; set; }

        [Required]
        [Display(Name = "Usuario que lo modifica")]
        public string ModifiedUser { get; set; }

        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Fecha de Modificacion")]
        public DateTime ModifiedDate { get; set; }


    }
}
