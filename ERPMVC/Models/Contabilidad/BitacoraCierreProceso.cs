using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class BitacoraCierreProcesos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProceso { get; set; }

        public int IdBitacoraCierre { get; set; }

        [ForeignKey("IdBitacoraCierre")]
        public BitacoraCierreContable BitacoraCierresContable { get; set; }

        public DateTime? FechaCierre { get; set; }

        public int PasoCierre { get; set; }

        public string Proceso { get; set; }

        public string Estatus { get; set; }

        public string Mensaje { get; set; }
        public string Asientos { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }
    }
}
