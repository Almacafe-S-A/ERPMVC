using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EmployeeAbsence
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 EmployeeAbsenceId { get; set; }

        [Display(Name = "Empleado")]
        public Int64 EmployeeId { get; set; }

        [Display(Name = "Empleado")]
        public string EmployeeName { get; set; }

        [Display(Name = "Tipo de Inasistencia")]
        public Int64 AbsenceTypeId {get;set;}

        [Display(Name = "Tipo de Inasistencia")]
        public string AbsenceTypeName { get; set; }

        [Display(Name = "Deducción")]
        public string DeductionIndicator { get; set; }

        [Display(Name = "Fecha Inicial")]
        public DateTime FechaInicial { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime FechaFinal { get; set; }

        [Display(Name = "Cantidad de dias")]
        public double QuantityDays { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
