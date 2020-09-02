using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Policy
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public string type { get; set; }

        [Required]
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Required]
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }


    }


}
