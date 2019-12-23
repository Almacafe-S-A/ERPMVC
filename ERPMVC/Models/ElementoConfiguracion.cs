using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ElementoConfiguracion
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public long IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Configuración")]
        public long? Idconfiguracion { get; set; }
        [ForeignKey("Idconfiguracion")]
        public GrupoConfiguracion GrupoConfiguracion { get; set; }
        [Display(Name = "Valor decimal")]
        public double? Valordecimal { get; set; }
        [Display(Name = "Valor cadena")]
        public string Valorstring { get; set; }

        [Display(Name = "Valor cadena 2")]
        public string Valorstring2 { get; set; } 
        public string Formula { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; } 

    }


}
