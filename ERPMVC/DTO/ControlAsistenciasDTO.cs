using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class ControlAsistenciasDTO : ControlAsistencias
    {
        public List<ControlAsistencias> _ControlAsistencias { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }

        
        public DateTime Dia1 { get; set; }
        public DateTime Dia2 { get; set; }
        public DateTime Dia3 { get; set; }
        public DateTime Dia4 { get; set; }
        public DateTime Dia5 { get; set; }
        public DateTime Dia6 { get; set; }
        public DateTime Dia7 { get; set; }
        public DateTime Dia8 { get; set; }
        public DateTime Dia9 { get; set; }
        public DateTime Dia10 { get; set; }
        public DateTime Dia11 { get; set; }
        public DateTime Dia12 { get; set; }
        public DateTime Dia13 { get; set; }
        public DateTime Dia14 { get; set; }
        public DateTime Dia15 { get; set; }
        public DateTime Dia16 { get; set; }
        public DateTime Dia17 { get; set; }
        public DateTime Dia18 { get; set; }
        public DateTime Dia19 { get; set; }
        public DateTime Dia20 { get; set; }
        public DateTime Dia21 { get; set; }
        public DateTime Dia22 { get; set; }
        public DateTime Dia23 { get; set; }
        public DateTime Dia24 { get; set; }
        public DateTime Dia25 { get; set; }
        public DateTime Dia26 { get; set; }
        public DateTime Dia27 { get; set; }
        public DateTime Dia28 { get; set; }
        public DateTime Dia29 { get; set; }
        public DateTime Dia30 { get; set; }
        public DateTime Dia31 { get; set; }


        public Int64 Dia1TA { get; set; }
        public Int64 Dia2TA { get; set; }
        public Int64 Dia3TA { get; set; }
        public Int64 Dia4TA { get; set; }
        public Int64 Dia5TA { get; set; }
        public Int64 Dia6TA { get; set; }
        public Int64 Dia7TA { get; set; }
        public Int64 Dia8TA { get; set; }
        public Int64 Dia9TA { get; set; }
        public Int64 Dia10TA { get; set; }
        public Int64 Dia11TA { get; set; }
        public Int64 Dia12TA { get; set; }
        public Int64 Dia13TA { get; set; }
        public Int64 Dia14TA { get; set; }
        public Int64 Dia15TA { get; set; }
        public Int64 Dia16TA { get; set; }
        public Int64 Dia17TA { get; set; }
        public Int64 Dia18TA { get; set; }
        public Int64 Dia19TA { get; set; }
        public Int64 Dia20TA { get; set; }
        public Int64 Dia21TA { get; set; }
        public Int64 Dia22TA { get; set; }
        public Int64 Dia23TA { get; set; }
        public Int64 Dia24TA { get; set; }
        public Int64 Dia25TA { get; set; }
        public Int64 Dia26TA { get; set; }
        public Int64 Dia27TA { get; set; }
        public Int64 Dia28TA { get; set; }
        public Int64 Dia29TA { get; set; }
        public Int64 Dia30TA { get; set; }
        public Int64 Dia31TA { get; set; }

        public int LlegadasTarde { get; set; }
        public int DiasLaborales { get; set; }
        public String PorcentajeLlegadasTarde { get; set; }

        [UIHint("ElementoConfiguracion")]
        public virtual ElementoConfiguracion ElementoConfiguracion { get; set; }
    }
}
