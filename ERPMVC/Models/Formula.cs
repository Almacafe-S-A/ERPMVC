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
        public long IdFormula { get; set; }
        public string NombreFormula { get; set; } 
        public string CalculoFormula { get; set; } 
        public long? IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public int? IdTipoFormula { get; set; } 
        public string NombreTipoformula { get; set; }
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; } 
        public string UsuarioCreacion { get; set; } 
        public string UsuarioModificacion { get; set; } 
      

    }
}
