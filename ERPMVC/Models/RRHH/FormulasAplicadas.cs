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
        [Display(Name = "Id Fórmula Aplicada")]
        public long IdFormulaAplicada { get; set; } // bigint
        [Display(Name = "Fórmula")]
        public long? IdFormula { get; set; } // bigint
        [Display(Name = "Nombre Fórmula")]
        public string NombreFormula { get; set; } // text
        [Display(Name = "Estado")]
        public long? IdEstado { get; set; } // bigint
        [Display(Name = "Nombre Estado")]
        public string Estado { get; set; }
        [Display(Name = "Empleado")]
        public long? IdEmpleado { get; set; }
        [Display(Name = "Fórmula Empleada")]
        public string FormulaEmpleada { get; set; }
        [Display(Name = "Multiplicar Por")]
        public decimal? MultiplicarPor { get; set; }
        [Display(Name = "Cálculo")]
        public long? IdCalculo { get; set; } 
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; } 
        public string UsuarioCreacion { get; set; } // text
        public string UsuarioModificacion { get; set; } // text

    }



}
