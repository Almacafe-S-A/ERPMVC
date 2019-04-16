using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPAPI.Models
{
    public class ShipmentType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipmentTypeId { get; set; }
        [Required]
        public string ShipmentTypeName { get; set; }
        public string Description { get; set; }
    }
}
