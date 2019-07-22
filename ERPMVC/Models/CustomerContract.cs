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

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Gerente general cliente")]
        public string CustomerManager { get; set; }

        [Display(Name = "RTN Gerente general cliente")]
        public string RTNCustomerManager { get; set; }

        [Display(Name = "Constitución cliente")]
        public string CustomerConstitution { get; set; }

        [Display(Name = "Cotización")]
        public Int64 SalesOrderId { get; set; }

        [Display(Name = "Gerente General")]
        public string Manager { get; set; }

        [Display(Name = "RTN Gerente General")]
        public string RTNMANAGER { get; set; }

        [Display(Name = "Tiempo de almacenaje")]
        public string StorageTime { get; set; }

        [Display(Name = "Recargo moratorio")]
        public double LatePayment { get; set; }

        [Display(Name = "Area utilizada")]
        public double UsedArea { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitOfMeasureId { get; set; }
        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Recepción de la mercadería")]
        public string Reception { get; set; }

        public string WareHouses { get; set; }

        [Display(Name = "Valor documento certificado de deposito")]
        public double ValueCD { get; set; }
        [Display(Name = "Valor del seguro")]
        public double ValueSecure { get; set; }

        [Display(Name = "Valor de la bascula")]
        public double ValueBascula { get; set; }


        [Display(Name = "Salario Delegado")]
        public double DelegateSalary { get; set; }

        [Display(Name = "Bodega Habilitada Requerimientos")]
        public string WarehouseRequirements { get; set; }

        [Display(Name = "Resolución")]
        public string Resolution { get; set; }

        [Display(Name = "Mercancías")]
        public string Mercancias { get; set; }

        [Display(Name = "Banda transportadora")]
        public double BandaTransportadora { get; set; }

        [Display(Name = "Horas Extras")]
        public double ExtraHours { get; set; }

        [Display(Name = "Alimentación")]
        public double FoodPayment { get; set; }

        [Display(Name = "Transporte")]
        public double Transport { get; set; }

        [Display(Name = "Porcentaje/Comisión")]
        public double Porcentaje1 { get; set; }

        [Display(Name = "Porcentaje 2/Comisión")]
        public double Porcentaje2 { get; set; }

        [Display(Name = "Fecha de contrato")]
        public DateTime FechaContrato { get; set; }

        [Display(Name = "Montacargas")]
        public double MontaCargas { get; set; }

        [Display(Name = "Mulas Hidráulicas")]
        public double MulasHidraulicas { get; set; }

        [Display(Name = "Papelería")]
        public double Papeleria { get; set; }


        [Display(Name = "Valor/Comisión 1")]
        public double Valor1 { get; set; }

        [Display(Name = "Valor/Comisión 2")]
        public double Valor2 { get; set; }

        //[EmailAddress(ErrorMessage ="Agregue una direccion de correo valida")]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Correo1 { get; set; }

        //[EmailAddress(ErrorMessage ="Agregue una direccion de correo valida")]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Correo2 { get; set; }

        //[EmailAddress(ErrorMessage ="Agregue una direccion de correo valida")]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Correo3 { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de Modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
