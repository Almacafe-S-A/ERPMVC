using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class EstadoDTO:Estados
    {
        public List<Estados> _Estados { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
    public class GrupoEstado
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }

    }
}
