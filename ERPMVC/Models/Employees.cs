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
        public long? Puesto { get; set; }
        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime? FechaIngreso { get; set; }
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
        [Display(Name = "Genero")]
        public string Genero { get; set; }
        [Required]
        [Display(Name = "Código Estado")]
        public long? IdEstado { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string NombreEstado { get; set; }
        [Required]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }
        [Required]
        [Display(Name = "País Id")]
        public long? IdPais { get; set; }
        [Required]
        [Display(Name = "País")]
        public string NombrePais { get; set; }
        [Required]
        [Display(Name = "Ciudad Id")]
        public long? IdCiudad { get; set; }
        [Required]
        [Display(Name = "Ciudad")]
        public string NombreCiudad { get; set; }
        [Required]
        [Display(Name = "Moneda de Salario")]
        public long? MonedaSalario { get; set; }
        [Required]
        [Display(Name = "Código de Usuario")]
        public string Userid { get; set; }
        
        [Display(Name = "Foto")]
        public string Foto { get; set; }
        [Required]
        [Display(Name = "Código de Banco")]
        public long? IdBanco { get; set; }
        [Required]
        [Display(Name = "Cuenta de Banco")]
        public string CuentaBanco { get; set; }


        [Display(Name = "Fecha de Fin de Contrato")]
        public DateTime FechaFinContrato { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Telefono")]
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
        [Display(Name = "Sucursal")]
        public string NombreSucursal { get; set; }
        [Required]
        [Display(Name = "Tipo de Contrato")]
        public int IdTipoContrato { get; set; }
        [Required]
        [Display(Name = "Departamento Id")]
        public int IdDepartamento { get; set; }
        [Required]
        [Display(Name = "Departamento")]
        public string NombreDepartamento { get; set; }


        [Display(Name = "Tipo de Sangre")]
        public string TipoSangre { get; set; }
        [Display(Name = "Nombre del Contacto de Emergencia")]
        public string NombreContacto { get; set; }
        [Display(Name = "Telefono")]
        public string TelefonoContacto { get; set; }
        

        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

    }
}
