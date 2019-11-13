using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EmployeeExtraHoursDetail
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 EmployeeExtraHoursDetailId { get; set; }

        [Display(Name = "Id horas extras")]
        public Int64 EmployeeExtraHoursId { get; set; }

        [Display(Name = "Hora de inicio")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Hora de fin")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Cantidad de horas")]
        public double QuantityHours { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
    }
}
