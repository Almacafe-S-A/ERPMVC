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

        // [UIHint("UserId")]
        //[Required]
        //public Guid UserId { get; set; }
        //[Required]
        ////[UIHint("Roledropdown")]
        //public Guid RoleId { get; set; }
        public string UserName { get; set; }

        public string RoleName { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }

        [Required]
        public string UsuarioModificacion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaModificacion { get; set; }

    }




}
