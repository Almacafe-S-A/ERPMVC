using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Escala
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdEscala { get; set; }
        [Display(Name = "Nombre de escala")]
        public string NombreEscala { get; set; }
        [Display(Name = "Descripción escala")]
        public string DescripcionEscala { get; set; }
        [Display(Name = "Valor inicial")]
        public decimal? ValorInicial { get; set; }
        [Display(Name = "Valor final")]
        public decimal? ValorFinal { get; set; }
        [Display(Name = "Porcentaje")]
        public decimal? Porcentaje { get; set; }
        [Display(Name = "Valor del calculo")]
        public decimal? ValorCalculo { get; set; }
        public long? Idpadre { get; set; }
        public long? IdEstado { get; set; } 
        public string Estado { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de mofificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de creación")]
        public string Usuariocreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string Usuariomodificacion { get; set; }

    }
}
