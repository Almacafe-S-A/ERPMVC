using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class NumeracionSAR
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // [Key]
        public Int64 IdNumeracion { get; set; }

        public Int64 IdCAI { get; set; }

        public string NoInicio { get; set; }

        public string NoFin { get; set; }

        public DateTime FechaLimite { get; set; }

        public int CantidadOtorgada { get; set; }

        public string SiguienteNumero { get; set; }
        public string PuntoEmision { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }


    }
}
