using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class HoraExtra
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long IdBiometrico { get; set; }

        [Required]
        public long IdEmpleado { get; set; }

        [Required]
        public int Horas { get; set; }

        [Required]
        public int Minutos { get; set; }
        public string Observaciones { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }

        public Biometrico Encabezado { get; set; }

        [ForeignKey("IdEmpleado")]
        public Employees Empleado { get; set; }

        [Required]
        public long IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public Estados Estado { get; set; }
        public string Estados { get; set; }

        public double HorasExtras { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double HorasExtrasBiometrico { get; set; }

    }
}
