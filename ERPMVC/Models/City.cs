using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class City
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Display(Name = "Ciudad")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "País")]
        public long? CountryId { get; set; }

        [Display(Name = "Departamento")]
        public long? StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }

        [Display(Name = "Empleados")]
        public virtual List<Employees> Employees { get; set; }

        [Display(Name = "Id de estado")]
        public Int64? IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [ForeignKey("IdEstado")]
        public Estados Estados { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }

        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de Modificación")]
        public string UsuarioModificacion { get; set; }
    }
}
