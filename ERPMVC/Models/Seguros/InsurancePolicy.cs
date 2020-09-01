using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InsurancePolicy
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 InsurancePolicyId { get; set; }

        [Required]
        public DateTime PolicyDate { get; set; }
        public DateTime PolicyDueDate { get; set; }
        [Required]
        public string PolicyNumber { get; set; }

        public int InsurancesId { get; set; }
        [Display(Name = "Nombre Aseguradora")]
        public string InsurancesName { get; set; }

        public bool Propias { get; set; }

        public bool? ExchangeDifferential { get; set; }

        [Display(Name = "Id Cliente")]
        public Int64? CustomerId { get; set; }
        public Customer Customer { get; set; }
        [Display(Name = "Nombre del cliente")]
        public string CustomerName { get; set; }

        public double ReductibleRate { get; set; }

        public double MerchandiseRate { get; set; }


        public double LpsAmount { get; set; }

        public double DollarAmount { get; set; }

        public double InsuredTotal { get; set; }

        public string AttachmentURL { get; set; }

        public string AttachementFileName { get; set; }

        public Int64? EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estados Estados { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
