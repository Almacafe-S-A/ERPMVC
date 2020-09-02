using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Formula
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long IdFormula { get; set; }
        [Display(Name = "Nombre Fórmula")]
        public string NombreFormula { get; set; }
        [Display(Name = "Calculo Fórmula")]
        public string CalculoFormula { get; set; }
        [Display(Name = "Estado")]
        public long? IdEstado { get; set; }
        public string NombreEstado { get; set; }
        [Display(Name = "Tipo Fórmula")]
        public int? IdTipoFormula { get; set; } 
        public string NombreTipoformula { get; set; }
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; } 
        public string UsuarioCreacion { get; set; } 
        public string UsuarioModificacion { get; set; } 
      

    }
}
