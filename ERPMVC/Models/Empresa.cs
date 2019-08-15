using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Empresa
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long IdEmpresa { get; set; }
        [Display(Name = "Nombre de empresa")]
        public string NombreEmpresa { get; set; }
        [Display(Name = "Nombre de contacto")]
        public string NombreContacto { get; set; }
        [Display(Name = "Teléfono ")]
        public string Telefono { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
