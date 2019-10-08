using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPMVC.Models
{
    public class HoursWorked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdHorastrabajadas { get; set; }
        public long? IdEmpleado { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public string NombreEmpleado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? EsFeriado { get; set; }
        public decimal? MultiplicaHoras { get; set; }

        #region Associations

        public List<HoursWorkedDetail> idhorastrabajadasconstrains { get; set; } = new List<HoursWorkedDetail>();

        #endregion

    }
}
