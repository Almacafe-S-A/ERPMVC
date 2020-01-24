using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

    
        public string UsuarioCreacion { get; set; }

  
        public string UsuarioModificacion { get; set; }

   
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

    }

    public class RolePermiso
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string Permiso { get; set; }

        [Required]
        public string Valor { get; set; }
    }

    public class RolesDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string NombreNormalizado { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int IdEstado { get; set; }
    }

}
