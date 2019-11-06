using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InsuranceEndorsementLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InsuranceEndorsementLineId { get; set; }

        public int InsuranceEndorsementId { get; set; }

        public string WarehouseName { get; set; }

        public int WareHouseId { get; set; }

        public double AmountLp { get; set; }

        public double AmountDl { get; set; }

        public double CertificateBalance { get; set; }

        public double AssuredDiference { get; set; }
    }
}
