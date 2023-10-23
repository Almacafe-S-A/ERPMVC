using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class DeduccionDTO
    {
        //[Required(ErrorMessage = "El código de tipo de deducción es requerido")]
        [Display(Name="Código Tipo Deducción")]
        public Int64 DeductionId { get; set; }
        //[Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        //[Required(ErrorMessage = "La categoría de deducción es requerida")]
        [Display(Name = "Categoría de Deducción")]
        //[Range(1,3,ErrorMessage = "La categoría de deducción puede ser: Por Ley, Eventual o Colegiación")]
        public Int64 DeductionTypeId { get; set; }
        public string DeductionType { get; set; }
        //[Required(ErrorMessage = "El valor es requerido")]
      //  [Display(Name = "Valor")]
       // public double ? Formula { get; set; }
        //[Required(ErrorMessage = "La quincena a aplicar es requerida")]
        [Display(Name = "Quincena a aplicar")]
       // [Range(1,3,ErrorMessage = "La quincena aplicar puede ser: 1era, 2da o ambas")]
        public double Fortnight { get; set; }
      //  public bool ? EsPorcentaje { get; set; }
        public string Cantidad { get; set; }
        // [Required]
        public long IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public Estados Estado { get; set; }
        public string NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
