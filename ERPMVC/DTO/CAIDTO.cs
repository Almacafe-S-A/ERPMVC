using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CAIDTO:CAI
    {
        public List<CAI> _CAIs { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
