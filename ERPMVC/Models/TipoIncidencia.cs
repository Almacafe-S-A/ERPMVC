using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPMVC.Models
{
    public class TipoIncidencia
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdTipoIncidencia { get; set; }
        public string NombreTipoIncidencia { get; set; }
        public long? IdEstado { get; set; }

        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
