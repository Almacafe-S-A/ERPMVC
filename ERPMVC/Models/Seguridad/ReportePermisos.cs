using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models.Seguridad
{
    public class ReportePermisos
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; set; }
        public int IdEstado { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? EstadoRolUser { get; set; }
        public DateTime? FechaCreacionRolUser { get; set; }
        public string UsuarioCreacionUser { get; set; }
        public DateTime? FechaModificacionUser { get; set; }
        public string UsuarioModificoUser { get; set; }
        public string ClaimType { get; set; }

        public string Nivel1 { get; set; }

        public string Nivel2 { get; set; }

        public string Nivel3 { get; set; }



    }
}
