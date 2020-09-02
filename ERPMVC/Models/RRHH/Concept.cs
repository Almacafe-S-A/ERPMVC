using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Concept
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id ")]
        public Int64 ConceptId { get; set; }

        [Display(Name = "Concepto")]
        public string ConceptName { get; set; }

        [Display(Name = "Id Tipo de Concepto")]
        public Int64 TypeId { get; set; }

        [Display(Name = "Tipo de Concepto")]
        public string TypeName { get; set; }

        [Display(Name = "Valor del concepto")]
        public double Value { get; set; }

        [Display(Name = "Cálculo del concepto")]
        public string Calculation { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }
    }
}
