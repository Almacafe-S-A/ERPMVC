using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class ControlPalletsDTO : ControlPallets
    {
        public int editar { get; set; } = 1;
       
        public double taracamion { get; set; }

        public double pesobruto { get; set; }

        public double pesoneto { get; set; }

        public double pesoneto2 { get; set; }

        public Boleto_Ent _Boleto_Ent { get; set; }
    }
}
