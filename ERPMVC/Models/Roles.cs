using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class RolPermisoAsignacion
    {
        public string Id { get; set; }
        public bool Asignado { get; set; }
        public string Categoria { get; set; }
        public string Nivel1 { get; set; }
        public string Nivel2 { get; set; }
        public string Nivel3 { get; set; }
        public string IdPadre { get; set; }
    }

    public class PostAsignacionesPermisoRol
    {
        public string IdRol { get; set; }
        public List<RolPermisoAsignacion> Permisos { get; set; }
    }
}
