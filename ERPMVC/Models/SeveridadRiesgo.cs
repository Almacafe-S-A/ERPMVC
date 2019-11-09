using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class SeveridadRiesgo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Codigó Severidad")]
        public Int64 IdSeveridad { get; set; }
        [Display(Name = "Impacto")]
        [Required]
        public Int64 Impacto { get; set; }
        [Display(Name = "Probabilidad")]
        [Required]
        public Int64 Probabilidad { get; set; }
        [Display(Name = "Limite Calidad Inferior")]
        [Required]
        public Int64 LimiteCalidadInferior { get; set; }
        [Display(Name = "Limete Calidad Superior")]
        [Required]
        public Int64 LimeteCalidadSuperir { get; set; }
        [Display(Name = "Rango Inferior de Severidad")]
        public double RangoInferiorSeveridad { get; set; }
        [Display(Name = "Rango Superior de Severidad")]
        public double RangoSuperiorSeveridad { get; set; }
        [Display(Name = "Nivel")]
        public string Nivel { get; set; }
        [Display(Name = "Color")]
        public string ColorHexadecimal { get; set; }
    }
}
