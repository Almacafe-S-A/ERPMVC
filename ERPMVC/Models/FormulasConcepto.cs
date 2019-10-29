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
         public long IdformulaConcepto { get; set; } // bigint
         public long? Idformula { get; set; } // bigint
         public long? IdConcepto { get; set; } // bigint
         public string NombreConcepto { get; set; } // text
         public DateTime? FechaCreacion { get; set; } // timestamp (6) without time zone
         public DateTime? FechaModificacion { get; set; } // timestamp (6) without time zone
         public string UsuarioCreacion { get; set; } // text
         public string UsuarioModificacion { get; set; } // text
         public string EstructuraConcepto { get; set; } // text
    }
}
