using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ContextoRiesgo
    {
        [Display(Name = "Id")]
        public Int64 IdContextoRiesgo { get; set; }
        [Display(Name = "Tipo de Contexto de Riesgo")]
        public string TipoContexto { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
