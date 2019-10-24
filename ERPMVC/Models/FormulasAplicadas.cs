using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class FormulasAplicadas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdFormulaAplicada { get; set; } // bigint
        public long? IdFormula { get; set; } // bigint
        public string NombreFormula { get; set; } // text
        public long? IdEstado { get; set; } // bigint
        public string Estado { get; set; } 
        public long? IdEmpleado { get; set; } 
        public string FormulaEmpleada { get; set; }   
        public decimal? MultiplicarPor { get; set; }
        public long? IdCalculo { get; set; } 
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; } 
        public string UsuarioCreacion { get; set; } // text
        public string UsuarioModificacion { get; set; } // text

    }



}
