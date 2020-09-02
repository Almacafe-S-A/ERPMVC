using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EmpleadoHorario
    {

        public long Id { get; set; }

        [Required]
        public long EmpleadoId { get; set; }
        [Required]
        public long HorarioId { get; set; }

        public Employees Empleado { get; set; }

        public Horario HorarioEmpleado { get; set; }

        [Required]
        public long IdEstado { get; set; }

        public Estados Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
