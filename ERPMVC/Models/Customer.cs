using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Cliente")]
        public Int64 CustomerId { get; set; }
        [Required]
        [Display(Name = "Razón o denominación social ")]
        public string CustomerName { get; set; }

        [Display(Name = "Identidad Gerente/Apoderado")]
        public string IdentidadApoderado { get; set; }

        [Display(Name = "Nombre Gerente/Apoderado")]
        public string NombreApoderado { get; set; }

        [Display(Name = "Número  de referencia de cliente")]
        public string CustomerRefNumber { get; set; }

        [Required]
        [Display(Name = "RTN del cliente")]
        public string RTN { get; set; }

        [Display(Name = "Tipo de cliente")]
        public long? CustomerTypeId { get; set; }

        [Display(Name = "Tipo de cliente")]
        public string CustomerTypeName { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "País")]
        public long? CountryId { get; set; }

        [Display(Name = "País")]
        public string CountryName { get; set; }

        [Display(Name = "Ciudad")]
        public long? CityId { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Departamento")]
        public long? StateId { get; set; }

        [Display(Name = "Departamento")]
        public string State { get; set; }
        [Display(Name = "Codigo Zip")]
        public string ZipCode { get; set; }

        [Display(Name = "Teléfonos")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Identidad del contacto")]
        public string Identidad { get; set; }

        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Email { get; set; }
        [Display(Name = "Persona de Contacto ")]
        public string ContactPerson { get; set; }

        [Display(Name = "Activo/Inactivo ")]
        public Int64? IdEstado { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Grupo económico")]
        public Int64? GrupoEconomicoId { get; set; }

        [Display(Name = "Grupo económico")]
        public string GrupoEconomico { get; set; }

        [Display(Name = "Monto de activos")]
        public double MontoActivos { get; set; }

        [Display(Name = "Ingresos anuales")]

     
        public double MontoIngresosAnuales { get; set; }

        [Display(Name = "Proveedor 1")]
        public string Proveedor1 { get; set; }

        [Display(Name = "Proveedor 2")]
        public string Proveedor2 { get; set; }

        [Display(Name = "Cliente pasara a recogerla a las oficinas de ALMACAFE")]
        public bool ClienteRecoger { get; set; }

        [Display(Name = "Enviarla con el mensajero")]
        public bool EnviarlaMensajero { get; set; }

        [Display(Name = "Dirección de envío")]
        public string DireccionEnvio { get; set; }

        [Display(Name = "Articulos pertenecen a la empresa u otra organización")]
        public string PerteneceEmpresa { get; set; }

        [Display(Name = "Confirmación por correo")]
        public bool ConfirmacionCorreo { get; set; }


        [Required]
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Required]
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        //  public List<CustomersOfCustomer> _Customers { get; set; }

        //        public List<VendorOfCustomer> _Vendor { get; set; }

    }
}
