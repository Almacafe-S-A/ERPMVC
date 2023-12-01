using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ControlAsistencias
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public Employees Empleado { get; set; }
        [Display(Name = "Empleado")]
        [ForeignKey("IdEmpleado")]
        public long IdEmpleado { get; set; }
        [Display(Name = "Dia")]
        public int Dia { get; set; }
        [Display(Name = "Tipo Asistencia")]
        public Int64 TipoAsistencia { get; set; }
        [Display(Name = "Usuario de creacion")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificacion")]
        public string UsuarioModificacion { get; set; }
        [Display(Name = "Fecha de creacion")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        public bool Revisado { get; set; }
        // public virtual List<ElementoConfiguracion> ElementoConfiguration { get; set; } = new List<ElementoConfiguracion>(
    }
}
