using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;




namespace ERPMVC.Models
{
    public class Puesto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPuesto { get; set; }
        public string NombrePuesto { get; set; }
        public long? IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }

        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}