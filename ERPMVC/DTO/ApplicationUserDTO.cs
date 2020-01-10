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
        [Display(Name = "Cambiar")]
        [DataType("Boolean")]
        public bool? cambiarpassword { get; set; }
    }


 
}
