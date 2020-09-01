using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class TipoBonificacion
    {
        public long Id { get; set; }

        public string Nombre { get; set; }

        public long EstadoId { get; set; }

        public Estados Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public string UsuarioCreacion { get; set; }
    }
}
