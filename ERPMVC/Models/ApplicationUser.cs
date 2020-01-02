using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Habilitado")]
        [DataType("Boolean")]
        public bool? IsEnabled { get; set; }

        [DataType("Password")]
        //[UIHint("Password")]
        public override string PasswordHash { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual List<PasswordHistory> PasswordHistory { get; set; } = new List<PasswordHistory>();
        [UIHint("Branches")]
        public virtual Branch Branch { get; set; }


    }
}
