using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerContract
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CustomerContractId { get; set; }

        [Display(Name = "Servicio")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Servicio")]
        public String ProductName { get; set; }


        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        public double? Plazo { get; set; }

        public double? IncrementoAnual { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Gerente general cliente")]
        public string CustomerManager { get; set; }

        public string TypeContractName { get; set; }

        [Display(Name = "RTN Gerente general cliente")]
        public string RTNCustomerManager { get; set; }

        [Display(Name = "Gerente general profesión/Nacionalidad cliente")]
        public string CustomerManagerProfesionNacionalidad { get; set; }

        [Display(Name = "Constitución cliente")]
        public string CustomerConstitution { get; set; }

        [Display(Name = "Cotización")]
        public Int64 SalesOrderId { get; set; }

        [Display(Name = "Gerente General")]
        public string Manager { get; set; }

        [Display(Name = "RTN Gerente General")]
        public string RTNMANAGER { get; set; }


        [Display(Name = "Tipo de contrato")]
        public Int64 TypeContractId { get; set; }

        [Display(Name = "Nombre de contrato")]
        public string NameContract { get; set; }

        [Display(Name = "Tipo de Facturación")]
        public Int64 TypeInvoiceId { get; set; }

        [Display(Name = "Tipo de Facturación")]
        public string TypeInvoiceName { get; set; }

        [Display(Name = "Tiempo de almacenaje")]
        public string StorageTime { get; set; }

        public string ContitucionAlmacafe { get; set; }

        public string RelacionPartes { get; set; }

        public string WareHouses { get; set; }


        [Display(Name = "Fecha de contrato")]
        public DateTime? FechaContrato { get; set; }

        public DateTime? FechaInicioContrato { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Papelería")]
        public double Papeleria { get; set; }


        public double? PrecioBaseProducto { get; set; }

        public double? ComisionMin { get; set; }

        public double? ComisionMax { get; set; }

        public double? PrecioServicio { get; set; }

        public bool? PolizaPropia { get; set; }


        //[EmailAddress(ErrorMessage ="Agregue una direccion de correo valida")]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Correo1 { get; set; }

        public string Observcion { get; set; }

        [Display(Name = "Estado")]
        public Int64? IdEstado { get; set; }

        public Estados Estados { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de Modificación")]
        public string UsuarioModificacion { get; set; }

        //LLave para el contrato de origen del adendum

        public Int64? CustomerContractId_Source { get; set; }
        [ForeignKey("CustomerContractId_Source")]
        public CustomerContract CustomerContractAdendum { get; set; }

        public int? CustomerContractType { get; set; }

        public string CustomerContractTypeName { get; set; }

        public int? AdendumNo { get; set; }

        public List<CustomerContractLines> customerContractLines { get; set; }

        public List<CustomerContractLinesTerms> customerContractLinesTerms { get; set; }

    }
}
