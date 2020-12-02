using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class PagoISR
    {
        public long Id { get; set; }
        public long Periodo { get; set; }
        public long EmpleadoId { get; set; }
        public double TotalAnual { get; set; }
        public double PagoAcumulado { get; set; }
        public double Saldo { get; set; }
        public long EstadoId { get; set; }
        public Estados Estado { get; set; }
        public Employees Empleado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }

    public class PagosISRDTO : PagoISR
    {
        public decimal CuotaISR { get; set; }
    }
}
