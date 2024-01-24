using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class SubServicesWareHouse
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 SubServicesWareHouseId { get; set; }

        [Display(Name = "Fecha de Subservicios")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Bodega")]
        public Int64 WareHouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WareHouseName { get; set; }

        [Display(Name = "Servicio")]
        public Int64 ServiceId { get; set; }

        [Display(Name = "Servicio")]
        public string ServiceName { get; set; }

        [Display(Name = "Sub Servicio")]
        public Int64 SubServiceId { get; set; }

        [Display(Name = "Sub Servicio")]
        public string SubServiceName { get; set; }

        public string Motivo { get; set; }

        public string Solicitante { get; set; }
        public bool CobroPorTiempo { get; set; }



        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Hora de inicio")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Hora de fin")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Cantidad de horas")]
        public double QuantityHours { get; set; }

        [Display(Name = "Empleado")]
        public Int64 EmployeeId { get; set; }

        [Display(Name = "Empleado")]
        public string EmployeeName { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
    }
}
