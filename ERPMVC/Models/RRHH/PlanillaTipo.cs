using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace ERPMVC.Models
{
    public class PlanillaTipo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long IdTipoPlanilla { get; set; }
        [Display(Name = "Tipo de Planilla")]
        public string TipoPlanilla { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Estado")]
        public long EstadoId { get; set; }
        [Display(Name = "Categoria")]
        public long CategoriaId { get; set; }

        public Estados Estado { get; set; }

        public CategoriaPlanilla Categoria { get; set; }

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