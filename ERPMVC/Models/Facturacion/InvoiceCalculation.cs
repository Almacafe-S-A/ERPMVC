using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InvoiceCalculation
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int64 InvoiceCalculationId { get; set; }
        [Display(Name = "Fecha de Emisión")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Factura proforma")]
        public Int64 ProformaInvoiceId { get; set; }

        [Display(Name = "Factura")]
        public Int64 InvoiceId { get; set; }

        [Display(Name = "IdCD")]
        public Int64 IdCD { get; set; }
        [Display(Name = "NoCD")]
        public Int64 NoCD { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Dias")]
        public int Dias { get; set; }

        [Display(Name = "Precio unitario")]
        public double UnitPrice { get; set; }

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Valor en Lps.")]
        public double ValorLps { get; set; }

        [Display(Name = "Valor a facturar")]
        public double ValorFacturar { get; set; }

        [Display(Name = "Valor a facturar")]
        public double IngresoMercadería { get; set; }

        [Display(Name = "Valor a facturar")]
        public double MercaderiaCertificada { get; set; }

        [Display(Name = "Dias")]
        public int Dias2 { get; set; }

        [Display(Name = "Porcentaje Merma")]
        public double PorcentajeMerma { get; set; }

        [Display(Name = "Valor en Lps.")]
        public double ValorLpsMerma { get; set; }

        [Display(Name = "Valor a Facturar")]
        public double ValorAFacturarMerma { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Identificador")]
        public Guid? Identificador { get; set; } 

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }    

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
    }
}
