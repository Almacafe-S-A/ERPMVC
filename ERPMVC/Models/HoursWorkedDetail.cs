using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class HoursWorkedDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdDetallehorastrabajadas { get; set; } 
        public long? IdHorasTrabajadas { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Horaentrada { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Horasalida { get; set; } 
        public decimal? Multiplicahoras { get; set; }
        public string UsuarioCreacion { get; set; } 
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; }

        #region Associations


        // public HorasTrabajadas IdHorasTrabajadasconstrain { get; set; }

        #endregion

    }
}
