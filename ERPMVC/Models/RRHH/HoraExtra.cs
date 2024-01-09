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
        public long DetalleBiometricoId { get; set; }
        [ForeignKey("DetalleBiometricoId")]
        public DetalleBiometrico DetalleBiometrico { get; set; }



        [Required]
        public long IdEmpleado { get; set; }

        [Required]
        public int Horas { get; set; }

        [Required]
        public int Minutos { get; set; }
        public int HoraAlumerzo { get; set; }

        public string Observaciones { get; set; }

        public DateTime? Fecha { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }



        [ForeignKey("IdEmpleado")]
        public Employees Empleado { get; set; }

        [Required]
        public long IdEstado { get; set; }
        public string Estados { get; set; }

        [ForeignKey("IdEstado")]
        public Estados Estado { get; set; }

        public double HorasExtras { get; set; }

        public double HorasTrabajadas { get; set; }

    }
}
