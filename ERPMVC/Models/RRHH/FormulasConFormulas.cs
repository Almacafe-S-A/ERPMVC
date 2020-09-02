using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class FormulasConFormulas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Fórmula Con Fórmula")]
        public long IdFormulaconformula { get; set; }
        [Display(Name = "Fórmula")]
        public long? IdFormula { get; set; }
        [Display(Name = "Fórmula Child")]
        public long? IdFormulachild { get; set; }
        [Display(Name = "Nombre Fórmula Child")]
        public string NombreFormulachild { get; set; }
        [Display(Name = "Estructura Concepto")]
        public string EstructuraConcepto { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }
}
