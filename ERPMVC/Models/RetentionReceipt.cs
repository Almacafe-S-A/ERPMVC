using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class RetentionReceipt
    {
        [Display(Name = "Id")]
        [Key]
        public int RetentionReceiptId { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Id Documento Asociado")]
        public Int64 DocumentId { get; set; }

        [ForeignKey("IdTipoDocumento")]
        [Display(Name = "Tipo de Documento Asociado")]
        public Int64 IdTipoDocumento { get; set; }

        [Display(Name = "No Correlativo")]
        public string NoCorrelativo { get; set; }

        [Display(Name = "CAI")]
        public string CAI { get; set; }

        [Display(Name = "Fecha de Emision")]
        public DateTime FechaEmision { get; set; }

        [Display(Name = "RTN")]
        public string RTN { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        [ForeignKey("VendorId")]
        [Display(Name = "Proveedor")]
        public Int64 VendorId { get; set; }
        [Display(Name = "CAI Proveedor")]
        public string VendorCAI { get; set; }
        [ForeignKey("IdEmpleado")]
        [Display(Name = "Empleado")]
        public long IdEmpleado { get; set; }
        [ForeignKey("BranchId")]
        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Nombre Sucursal")]
        public string BranchName { get; set; }
        [Display(Name = "Codigo Sucursal")]
        public string BranchCode { get; set; }

        [ForeignKey("IdPuntoEmision")]
        [Display(Name = "Punto de emisión")]
        public Int64 IdPuntoEmision { get; set; }

        public Int64 IdEstado { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Numero de Factura")]
        public int NumeroDEI { get; set; }

        [Display(Name = "Descripcion Impuesto Retenido")]
        public string RetentionTaxDescription { get; set; }

        [Display(Name = "Base Imponible")]
        public double TaxableBase { get; set; }

        [Display(Name = "Porcentaje")]
        public double Percentage { get; set; }

        [Display(Name = "Importe Total")]
        public double TotalAmount { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
