using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class PolicyRoles
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Id Politica")]
        public Guid IdPolicy { get; set; }
        [ForeignKey("IdPolicy")]
        public Policy Policy { get; set; }

        public string PolicyName { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public Guid IdRol { get; set; }
        [ForeignKey("IdRol")]
        public ApplicationRole Role { get; set; }


        public string RolName { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }

        [Required]
        public string UsuarioModificacion { get; set; }

    }


}
