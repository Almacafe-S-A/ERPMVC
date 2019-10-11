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
        [Display(Name = "Código Horas Trabajadas Detalle")]
        public long IdDetallehorastrabajadas { get; set; }
        [Display(Name = "Código Horas Trabajadas")]
        public long? IdHorasTrabajadas { get; set; }
        [Display(Name = "Hora de Entrada")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:¨dd/MM/yyyy HH:mm:ss}")]
        public DateTime? Horaentrada { get; set; }
        [Display(Name = "Hora de Salida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:¨dd/MM/yyyy HH:mm:ss}")]
        public DateTime? Horasalida { get; set; }
        [Display(Name = "Multiplica Horas Por:")]
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
