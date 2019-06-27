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
        public string NombreEscala { get; set; }
        public string DescripcionEscala { get; set; } 
        public decimal? ValorInicial { get; set; }
        public decimal? ValorFinal { get; set; } 
        public decimal? Porcentaje { get; set; } 
        public decimal? ValorCalculo { get; set; }
        public long? Idpadre { get; set; }
        public long? IdEstado { get; set; } 
        public string Estado { get; set; } 
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; } 
        public string Usuariocreacion { get; set; } 
        public string Usuariomodificacion { get; set; }

    }
}
