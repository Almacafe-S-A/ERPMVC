using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    [DisplayColumn("Name")]
    public partial class State
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long Id { get; set; } // bigint
        [Display(Name = "Departamento")]
        public string Name { get; set; } // text


        [Display(Name = "Usuario de creación")]
        public string Usuariocreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string Usuariomodificacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }

        [Display(Name = "Estado")]
        [DefaultValue(1)]
        public Int64 IdEstado { get; set; } = 1;

        public Int64 CountryId { get; set; }

        public List<City> City { get; set; }
    }
}
