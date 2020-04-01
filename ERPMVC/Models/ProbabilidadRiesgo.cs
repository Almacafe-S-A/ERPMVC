using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ProbabilidadRiesgo
    {
        [Display(Name = "Id")]
        public Int64 Id { get; set; }
        [Display(Name = "Nivel")]
        public Int64 Nivel { get; set; }
        [Display(Name = "Descriptor")]
        public string Descriptor { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Frecuencia")]
        public string Frecuencia { get; set; }
        public string ColorHexadecimal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
