using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GuiaRemision
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NumeroDocumento { get; set; }

        public string CAI { get; set; }

        public string Rango { get; set; }

        public DateTime FechaLimiteEmision { get; set; }

        public DateTime Fecha { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        //Destinatario
        public string CustomerName { get; set; }

        public string Remitente { get; set; }

        public string Origen { get; set; }

        public string Destino { get; set; }

        public string Transportista { get; set; }

        public string Vigilante { get; set; }

        public string Observaciones { get; set; }

        public string Marca { get; set; }

        public string Placa { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public List<GuiaRemisionLine> GuiaRemisionLines { get; set; }
    }

    public class GuiaRemisionLine {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int GuiaRemisionId { get; set; }
        [ForeignKey("GuiaRemisionId")]
        public GuiaRemision GuiaRemision { get; set; }

        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }
        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64? UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Cantidad")]
        public decimal Quantity { get; set; }
    }
}
