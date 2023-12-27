using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Models;

namespace ERPMVC.Models
{
    public class DeduccionEmpleado
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
        [Required]
        public long EmpleadoId { get; set; }

        [ForeignKey("EmpleadoId")]
        public Employees Empleado { get; set; }

        public string NombreEmpleado { get; set; }

        public Int64 TipoDeduccionId { get; set; }
        [ForeignKey("TipoDeduccionId")]
        public TipoDeduccion TipoDeduccion { get; set; }
        public DateTime Fecha { get; set; }
        
        public int Mes { get; set; }
        
        public int PeriodoId { get; set; }
        [ForeignKey("PeriodoId")]
        public Periodo Periodo { get; set; }
        [Required]
        public int Quincena { get; set; }
        [Required]
        public decimal Cantidad { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public float Monto { get; set; }

        public int CuotaNo { get; set; }
        [Required]
        public long EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estados Estados { get; set; }

        public int CantidadCuotas { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }


    }

    public class DeduccionesEmpleadoDTO: DeduccionEmpleado
    {
    
        public int CantidadDeducciones { get; set; }
        public float TotalDeducciones { get; set; }
        public double SalarioEmpleado { get; set; }

    }
}
