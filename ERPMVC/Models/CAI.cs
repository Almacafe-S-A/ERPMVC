using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{

    public class CAI
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IdCAI { get; set; }
        public string _cai { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaLimiteEmision { get; set; }

        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }
 
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
