using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class DeduccionDTO
    {
        [Required(ErrorMessage = "El código de tipo de deducción es requerido")]
        [Display(Name="Código Tipo Deducción")]
        public Int64 DeductionId { get; set; }
        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Required(ErrorMessage = "La categoría de deducción es requerida")]
        [Display(Name = "Categoría de Deducción")]
        [Range(1,2,ErrorMessage = "La categoría de deducción puede ser: Por Ley o Eventual")]
        public Int64 DeductionTypeId { get; set; }
        public string DeductionType { get; set; }
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Valor")]
        public double Formula { get; set; }
        [Required(ErrorMessage = "La quincena a aplicar es requerida")]
        [Display(Name = "Quincena a aplicar")]
        [Range(1,2,ErrorMessage = "La quincena aplicar puede ser: 1era o 2da")]
        public double Fortnight { get; set; }
        public bool EsPorcentaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
