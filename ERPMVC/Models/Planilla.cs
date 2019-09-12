using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;




namespace ERPMVC.Models
{
    public class Planilla
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long IdPlanilla { get; set; }
        [Display(Name = "Tipo de Planilla")]
        [Required]
        public string TipoPlanilla { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }
        [Display(Name = "Estado")]
        [Required]
        public long EstadoId { get; set; }


        [Display(Name = "Usuario de creación")]
        public string Usuariocreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string Usuariomodificacion { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }
    }
}