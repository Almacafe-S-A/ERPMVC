using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.DTO;

namespace ERPMVC.Models
{
    public class DeduccionEmpleado
    {
        [Required]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "El empleado es requerido")]
        [Display(Name = "Empleado")]
        public long EmpleadoId { get; set; }

        [Required(ErrorMessage = "El tipo de deducción es requerido")]
        [Display(Name = "Tipo de Deducción")]
        public Int64 DeductionId { get; set; }

        [Required(ErrorMessage = "El monto es requerido.")]
        [Display(Name = "Monto")]
        public float Monto { get; set; }

        [Display(Name = "Fecha Inicio")]
        public DateTime VigenciaInicio { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime VigenciaFinaliza { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }

        [Display(Name = "Cuotas")]
        public int CantidadCuotas { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }

    public class DeduccionesEmpleadoDTO
    {
        public long EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; }
        public int CantidadDeducciones { get; set; }
        public float TotalDeducciones { get; set; }
    }
}
