
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Liquidacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime FechaLiquidacion { get; set; }

        public Int64 CustomerId { get; set; }
        public Customer Customer { get; set; }


        public string CustomerName { get; set; }

        


        public Int64 ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }


        public string ProductName { get; set; }
        public string Recibos { get; set; }

        public double TasaCambio { get; set; }

        public double DerechosImportacion { get; set; }

        public double SelectivoConsumo { get; set; }

        public double ImpuestoSobreVentas { get; set; }


        public double Flete { get; set; }

        public double Seguro { get; set; }

        public double Otros { get; set; }

        public double Total { get; set; }



        public Int64 EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estados Estados { get; set; }

        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de Creación")]
        public string UsuarioModificacion { get; set; }

        public List<LiquidacionLine> detalleliquidacion { get; set; }





    }
}
