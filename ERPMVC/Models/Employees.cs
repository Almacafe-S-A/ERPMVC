using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Employees
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long IdEmpleado { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string NombreEmpleado { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Correo { get; set; }
        [Required]
        [Display(Name = "Puesto")]
        [ForeignKey("PuestoId")]
        public Puesto Puesto { get; set; }
        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime? FechaIngreso { get; set; }
        public string BirthPlace { get; set; }
        [Required]
        [Display(Name = "Salario")]
        public decimal? Salario { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Required]
        [Display(Name = "Identidad")]
        public string Identidad { get; set; }
        [Display(Name = "Fecha de Egreso")]
        public DateTime? FechaEgreso { get; set; }
        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Required]
        [Display(Name = "Género")]
        public string Genero { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public long? IdEstado { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string NombreEstado { get; set; }
        [Required]
        [Display(Name = "Activo-Inactivo")]
        public long? IdActivoinactivo { get; set; }
        [Required]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }
        [Required]
        [Display(Name = "País Id")]
        public long? IdCountry { get; set; }
        [Required]
        [Display(Name = "Ciudad Id")]
        public long? IdCity { get; set; }
        [Display(Name = "Horas Extra")]
        public bool? HorasExtra { get; set; }
        [Required]
        [Display(Name = "Moneda de Salario")]
        public long? IdCurrency { get; set; }
        [Required]
        [Display(Name = "Código de Usuario")]
        public string ApplicationUserId { get; set; }    
        [Display(Name = "Foto")]
        public string Foto { get; set; }
        [Required]
        [Display(Name = "Código de Banco")]
        public long? IdBank { get; set; }
        [Required]
        [Display(Name = "Cuenta de Banco")]
        public string CuentaBanco { get; set; }
        [Display(Name = "Fecha de Fin de Contrato")]
        public DateTime? FechaFinContrato { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        [Display(Name = "Extensión")]
        public int Extension { get; set; }
        [Display(Name = "Notas")]
        public string Notas { get; set; }
        [Display(Name = "Puesto Id")]
        public int IdPuesto { get; set; }
        [Required]
        [Display(Name = "Nombre del Puesto")]
        public string NombrePuesto { get; set; }
        [Display(Name = "Sucursal Id")]
        public int IdSucursal { get; set; }
        [Required]
        [Display(Name = "Tipo de Contrato")]
        public int IdTipoContrato { get; set; }
        [Required]
        [Display(Name = "Departamento Id")]
        public int IdDepartamento { get; set; }
        [Required]
        [Display(Name = "Departamento")]
        public long? IdState { get; set; }
        [Required]
        [Display(Name = "Tipo de Sangre")]
        public string TipoSangre { get; set; }
        [Display(Name = "Nombre del Contacto de Emergencia")]
        public string NombreContacto { get; set; }
        [Display(Name = "Teléfono")]
        public string TelefonoContacto { get; set; }
        public int IdBranch { get; set; }
        public string RTN { get; set; }
        [Display(Name = "Tipo Planilla")]
        public long? IdTipoPlanilla { get; set; }
        [ForeignKey("IdTipoPlanilla")]
        public TipoPlanillas TipoPlanilla { get; set; }
        [Display(Name = "Profesión")]
        public string Profesion { get; set; }
        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public List<EmployeeSalary> _EmployeeSalary { get; set; } = new List<EmployeeSalary>();
        

    }
}
