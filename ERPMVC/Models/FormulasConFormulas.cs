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
        public long IdFormulaconformula { get; set; }
        public long? IdFormula { get; set; }
        public long? IdFormulachild { get; set; }
        public string NombreFormulachild { get; set; }
        public string EstructuraConcepto { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }
}
