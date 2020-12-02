using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class FormulasConcepto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Fórmula Concepto")]
        public long IdformulaConcepto { get; set; } // bigint
        [Display(Name = "Fórmula")]
        public long? IdFormula { get; set; } // bigint
        [Display(Name = "Concepto")]
        public long? IdConcepto { get; set; } // bigint
        [Display(Name = "Nombre Concepto")]
        public string NombreConcepto { get; set; } // text
         public DateTime? FechaCreacion { get; set; } // timestamp (6) without time zone
         public DateTime? FechaModificacion { get; set; } // timestamp (6) without time zone
         public string UsuarioCreacion { get; set; } // text
         public string UsuarioModificacion { get; set; } // text
        [Display(Name = "Estructura Concepto")]
        public string EstructuraConcepto { get; set; } // text
    }
}
