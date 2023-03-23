using ERPMVC.Models.Clientes;
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

        [Display(Name = "RTN Gerente General")]
        public string RTNGerenteGeneral { get; set; }

        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Email { get; set; }
        [Display(Name = "Persona de Contacto ")]
        public string ContactPerson { get; set; }

        public string NombreEmpresaPN { get; set; }

        public string TelefonoEmpresaPN { get; set; }

        public string DireccionEmpresaPN { get; set; }

        public bool PEP { get; set; }

        public bool AFND { get; set; }


        [Display(Name = "Activo/Inactivo ")]
        public Int64? IdEstado { get; set; }

        public int? UnitOfMeasurePreference { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Grupo económico")]
        public Int64? GrupoEconomicoId { get; set; }

        [Display(Name = "Grupo económico")]
        public string GrupoEconomico { get; set; }

        [Display(Name = "Monto de activos")]
        public double? MontoActivos { get; set; }

        [Display(Name = "Ingresos anuales")]

     
        public double MontoIngresosAnuales { get; set; }

        [Display(Name = "Proveedor 1")]
        public string Proveedor1 { get; set; }

        [Display(Name = "Proveedor 2")]
        public string Proveedor2 { get; set; }

        [Display(Name = "Cliente pasara a recogerla a las oficinas de ALMACAFE")]
        public bool? ClienteRecoger { get; set; }

        [Display(Name = "Enviarla con el mensajero")]
        public bool? EnviarlaMensajero { get; set; }

        [Display(Name = "Dirección de envío")]
        public string DireccionEnvio { get; set; }

        [Display(Name = "Articulos pertenecen a la empresa u otra organización")]
        public string PerteneceEmpresa { get; set; }

        public decimal? ValorSeveridadRiesgo { get; set; }

        public string NivelSeveridad { get; set; }

        public string ColorHexadecimal { get; set; }

        [Display(Name = "Confirmación por correo")]
        public bool? ConfirmacionCorreo { get; set; }

        public Int64? ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { get; set; }


        [Required]
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Required]
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }

        [Required]
        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }

        //  public List<CustomersOfCustomer> _Customers { get; set; }

        //        public List<VendorOfCustomer> _Vendor { get; set; }

        public string LugarNacimiento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Edad { get; set; }
        public EstadoCivilEnum? EstadoCivil { get; set; }
        public string ProfesionOficio { get; set; }
        public string Nacionalidad { get; set; }
        public string GiroActividadNegocio { get; set; }
        public bool? CargosPublicos { get; set; }
        public string Familiares { get; set; }
        public string Conyugue { get; set; }
        public bool? InstitucionSupervisada { get; set; }
        public string NombreFuncionario { get; set; }
        public string FirmaAuditoriaExterna { get; set; }

        public Customer(dynamic dto)
        {
            try
            {
               
                this.CustomerId = dto.CustomerId;
                this.CustomerName = dto.CustomerName;
                this.IdentidadApoderado = dto.IdentidadApoderado;
                this.NombreApoderado = dto.NombreApoderado;
                this.CustomerRefNumber = dto.CustomerRefNumber;
                this.RTN = dto.RTN;
                this.CustomerTypeId = dto.CustomerTypeId;
                this.CustomerTypeName = dto.CustomerTypeName;
                this.Address = dto.Address;
                this.CountryId = dto.CountryId;
                this.CountryName = dto.CountryName;
                this.CityId = dto.CityId;
                this.City = dto.City;
                this.StateId = dto.StateId;
                this.State = dto.State;
                this.ZipCode = dto.ZipCode;
                this.Phone = dto.Phone;
                this.RTNGerenteGeneral = dto.RTNGerenteGeneral;
                this.Email = dto.Email;
                this.ContactPerson = dto.ContactPerson;
                this.IdEstado = dto.IdEstado;
                this.UnitOfMeasurePreference = dto.UnitOfMeasurePreference;
                //this.UnitOfMeasure = dto.UnitOfMeasure;
                this.Estado = dto.Estado;
                this.GrupoEconomicoId = dto.GrupoEconomicoId;
                this.GrupoEconomico = dto.GrupoEconomico;
                this.MontoActivos = dto.MontoActivos;
                this.MontoIngresosAnuales = dto.MontoIngresosAnuales;
                this.Proveedor1 = dto.Proveedor1;
                this.Proveedor2 = dto.Proveedor2;
                this.ClienteRecoger = dto.ClienteRecoger;
                this.EnviarlaMensajero = dto.EnviarlaMensajero;
                this.DireccionEnvio = dto.DireccionEnvio;
                this.PerteneceEmpresa = dto.PerteneceEmpresa;
                this.ValorSeveridadRiesgo = dto.ValorSeveridadRiesgo;
                this.NivelSeveridad = dto.NivelSeveridad;
                this.ColorHexadecimal = dto.ColorHexadecimal;
                this.ConfirmacionCorreo = dto.ConfirmacionCorreo;
                this.ProductTypeId = dto.ProductTypeId;
                this.ProductType = dto.ProductType;
                this.UsuarioCreacion = dto.UsuarioCreacion;
                this.UsuarioModificacion = dto.UsuarioModificacion;
                this.FechaCreacion = dto.FechaCreacion;
                this.FechaModificacion = dto.FechaModificacion;
                this.LugarNacimiento = dto.LugarNacimiento;
                this.FechaNacimiento = dto.FechaNacimiento;
                this.Edad = dto.Edad;
                this.EstadoCivil = dto.EstadoCivil;
                this.ProfesionOficio = dto.ProfesionOficio;
                this.Nacionalidad = dto.Nacionalidad;
                this.GiroActividadNegocio = dto.GiroActividadNegocio;
                this.CargosPublicos = dto.CargosPublicos;
                this.Familiares = dto.Familiares;
                this.Conyugue = dto.Conyugue;
                this.InstitucionSupervisada = dto.InstitucionSupervisada;
                this.NombreFuncionario = dto.NombreFuncionario;
                this.FirmaAuditoriaExterna = dto.FirmaAuditoriaExterna;
                
            }
            catch (Exception ex)
            {
                
                throw;
            }
            

        }
        public Customer()
        {
            

        }
    }
}
