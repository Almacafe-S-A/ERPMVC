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
        public int Id { get; set; }

        public Int64 CustomerId { get; set; }

        public string Customername { get; set; }

        public int WarehouseID { get; set; }

        public string WarehouseName { get; set; }

        public int WharehouseTypeId { get; set; }

        public string WarehouseTypeName { get; set; }

        public DateTime DateGenerated { get; set; }

        public int InsurancePolicyId { get; set; }

        public int ProductdId { get; set; }

        public string ProductName { get; set; }

        public double TotalAmountLp { get; set; }

        public double TotalAmountDl { get; set; }

        public double CertificateBalalnce { get; set; }

        public double AssuredDifernce { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public List<InsuranceEndorsementLine> InsuranceEndorsementLines { get; set; }
    }
}
