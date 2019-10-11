using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPMVC.Models
{
    public class HoursWorked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Código Horas Trabajadas")]
        public long IdHorastrabajadas { get; set; }
        [Display(Name = "Empleado")]
        public long? IdEmpleado { get; set; }
        [Display(Name = "Fecha de Entrada")]
        public DateTime? FechaEntrada { get; set; }
        [Display(Name = "Nombre Empleado")]
        public string NombreEmpleado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Es Feriado")]
        public bool? EsFeriado { get; set; }
        [Display(Name = "Multiplicar Horas por:")]
        public decimal? MultiplicaHoras { get; set; }

        #region Associations

        public List<HoursWorkedDetail> idhorastrabajadasconstrains { get; set; } = new List<HoursWorkedDetail>();

        #endregion

    }
}
