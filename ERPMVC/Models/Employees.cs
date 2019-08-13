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
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string Correo { get; set; }
        public long? Puesto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public decimal? Salario { get; set; }
        public string Estado { get; set; }
        public string Identidad { get; set; }
        public DateTime? FechaEgreso { get; set; }
        public string Direccion { get; set; }
        public string Genero { get; set; }
        public long? IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public string Ciudad { get; set; }
        public long? IdPais { get; set; }
        public string NombrePais { get; set; }
        public long? IdCiudad { get; set; }
        public string NombreCiudad { get; set; }
        public long? MonedaSalario { get; set; }
        public string Userid { get; set; }
        public Int64 Idsescalas { get; set; }
        public long? IdActivoinactivo { get; set; }
        public string Foto { get; set; }
        public long? IdBanco { get; set; }
        public string CuentaBanco { get; set; }

        public DateTime FechaFinContrato { get; set; }
        public string Telefono { get; set; }
        public int Extension { get; set; }
        public string Notas { get; set; }
        public int IdPuesto { get; set; }
        public string NombrePuesto { get; set; }
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public int IdTipoContrato { get; set; }
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }


        public string Usuariocreacion { get; set; }
        public string Usuariomodificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

    }
}
