using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{

    public class ApplicationUserDTO : ApplicationUser
    {
        //[Display(Name = "Reestablecer")]
        //[DataType("Boolean")]
        //public bool? cambiarpassword { get; set; }
        [Display(Name = "Bloqueado")]
        [DataType("Boolean")]
        public bool? IsBlocked { get; set; }
    }


 
}
