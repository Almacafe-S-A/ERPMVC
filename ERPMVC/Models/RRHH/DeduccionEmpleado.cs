using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ERPMVC.Models
{
    public class DeduccionEmpleado
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
        [Required]
        [UIHint("Employeesddl")]
        public long EmpleadoId { get; set; }

        [ForeignKey("EmpleadoId")]
        public Employees Empleado { get; set; }

        public string NombreEmpleado { get; set; }
        [UIHint("TipoDeduccion")]
        [Required]
        public Int64 TipoDeduccionId { get; set; }
        [ForeignKey("TipoDeduccionId")]
        public TipoDeduccion TipoDeduccion { get; set; }
        public DateTime Fecha { get; set; }

        public string FechaBonoString { get; set; }

        public int Mes { get; set; }
        
        public int PeriodoId { get; set; }
        [ForeignKey("PeriodoId")]
        public Periodo Periodo { get; set; }
        [UIHint("Quincena")]
        [Required]
        public int Quincena { get; set; }
        [Required]
        public decimal Cantidad { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public float Monto { get; set; }

        public int CuotaNo { get; set; }
        [UIHint("EstadosBonificacion")]
        [Required]
        public long EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estados Estados { get; set; }

        public int CantidadCuotas { get; set; }

        public string TipoDeduccionNombre { get; set; }
        public int Anio { get; set; }
        public string QuincenaNombre { get; set; }
        public string EstadoNombre { get; set; }

        public long? PlanillaDetalleId { get; set; }
        [ForeignKey("PlanillaDetalleId")]

        public PlanillaDetalle PlanillaDetalle { get; set; }




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
        public int IdPeriodo { get; set; }

        public int NoMes { get; set; }

    }


}
