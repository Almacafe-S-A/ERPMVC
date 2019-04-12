using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class SalesOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Display(Name = "Id")]
        public int SalesOrderId { get; set; }

        [Display(Name = "Nombre Solicitante")]
        public string SalesOrderName { get; set; }

        [Display(Name = "RTN")]
        public string RTN { get; set; }

        [Display(Name = "Telefono")]
        public string Tefono { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress]
        public string Correo { get; set; }

        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }
        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

         [Display(Name = "Fecha de cotizacion")]
        //[JsonProperty("revisedDate", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(FixedIsoDateTimeOffsetConverter))]
        public DateTime OrderDate { get; set; }
         [Display(Name = "Fecha de entrega")]
        public DateTime DeliveryDate { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }

        [Display(Name = "Numero de referencia de cliente")]
        public string CustomerRefNumber { get; set; }
        [Display(Name = "Tipo de ventas")]
        public int SalesTypeId { get; set; }
        public string Remarks { get; set; }

        [Display(Name = "Monto")]
        public double Amount { get; set; }
        public double SubTotal { get; set; }
         [Display(Name = "Descuento")]
        public double Discount { get; set; }

         [Display(Name = "Impuesto")]
        public double Tax { get; set; }
        [Display(Name = "Flete")]
        public double Freight { get; set; }
        public double Total { get; set; }

        [Display(Name = "Total exento")]
        public double TotalExento { get; set; }

        [Display(Name = "Total exonerado")]
        public double TotalExonerado { get; set; }

        [Display(Name = "Total Gravado")]
        public double TotalGravado { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();
    }


    public class FixedIsoDateTimeOffsetConverter : IsoDateTimeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?);
        }

        public FixedIsoDateTimeOffsetConverter() : base()
        {
            DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal;
        }
    }
}
