using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class BitacoraCierreContable
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Anio { get; set; }

        public int? Mes { get; set; }

        public int? PeriodoId { get; set; }

        [ForeignKey("PeriodoId")]
        public Periodo Periodo { get; set; }

        public DateTime? PeriodoMes { get; set; }

        public DateTime? FechaCierre { get; set; }

        public Int64 EstatusId { get; set; }

        public string Estatus { get; set; }

        public string Mensaje { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public List<BitacoraCierreProcesos> CierreContableLineas { get; set; }
    }
}
