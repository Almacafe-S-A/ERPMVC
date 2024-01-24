using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ERPMVC.DTO
{
    public class TipoPlanillasDTO : PlanillaTipo
    {
        public List<PlanillaTipo> _TipoPlanillas { get; set; }


        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}

