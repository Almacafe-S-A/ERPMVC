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

        [ForeignKey("IdEmpleado")]
        [Display(Name = "Empleado")]
        public long IdEmpleado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
