using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPMVC.Models
{
    public class Dependientes
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long IdDependientes { get; set; }
        [Display(Name = "Nombre de dependientes")]
        public string NombreDependientes { get; set; }
        [Display(Name = "Parentezco")]
        public string Parentezco { get; set; }

        [Display(Name = "Teléfono")]
        public string TelefonoDependientes { get; set; }

        [Display(Name = "Dirección")]
        public string DireccionDependientes { get; set; }

        [Display(Name = "Empleado")]
        public long? IdEmpleado { get; set; }

        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}

