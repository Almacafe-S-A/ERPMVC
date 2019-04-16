using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPAPI.Models
{
    public class Shipment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipmentId { get; set; }
        [Display(Name = "Shipment Number")]
        public string ShipmentName { get; set; }
        [Display(Name = "Sales Order")]
        public int SalesOrderId { get; set; }
        public DateTime ShipmentDate { get; set; }
        [Display(Name = "Shipment Type")]
        public int ShipmentTypeId { get; set; }
        [Display(Name = "Warehouse")]
        public int WarehouseId { get; set; }
        [Display(Name = "Full Shipment")]
        public bool IsFullShipment { get; set; } = true;
    }
}
