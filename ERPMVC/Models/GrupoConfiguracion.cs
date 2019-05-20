using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GrupoConfiguracion
    {
        [Key]
        public long IdConfiguracion { get; set; } 
        public string Nombreconfiguracion { get; set; }      
        public string Tipoconfiguracion { get; set; } 
        public long? IdConfiguracionorigen { get; set; } 
        public long? IdConfiguraciondestino { get; set; } 
        public long IdZona { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
