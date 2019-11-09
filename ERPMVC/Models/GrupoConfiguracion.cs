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
        [Display(Name = "Id Configuración")]
        public long IdConfiguracion { get; set; }
        [Display(Name = "Nombre Configuración")]
        public string Nombreconfiguracion { get; set; }
        [Display(Name = "Tipo Configuración")]
        public string Tipoconfiguracion { get; set; }
        [Display(Name = "Configuración Origen")]
        public long? IdConfiguracionorigen { get; set; }
        [Display(Name = "Configuración Destino")]
        public long? IdConfiguraciondestino { get; set; }
        [Display(Name = "Zona")]
        public long IdZona { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
