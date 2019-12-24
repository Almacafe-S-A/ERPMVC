using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InsuranceEndorsement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InsuranceEndorsementId { get; set; }

        public Int64 CustomerId { get; set; }

        public string Customername { get; set; }
        [Display(Name = "Centro de costos")]
        public Int64 CostCenterId { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public int WarehouseTypeId { get; set; }

        public string WarehouseTypeName { get; set; }

        public DateTime DateGenerated { get; set; }

        public int InsurancePolicyId { get; set; }

        public int ProductdId { get; set; }

        public string ProductName { get; set; }

        public double TotalAmountLp { get; set; }

        public double TotalAmountDl { get; set; }

        public double TotalCertificateBalalnce { get; set; }

        public double TotalAssuredDifernce { get; set; }

        public Int64 EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estados Estados { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public List<InsuranceEndorsementLine> InsuranceEndorsementLines { get; set; }
    }
}
