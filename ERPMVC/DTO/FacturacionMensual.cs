using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public partial class FacturacionMensual
    {
        public Nullable<int> FacturacionID { get; set; }
        public Nullable<double> Facturacion { get; set; }
        public System.DateTime Date { get; set; }
    }


}
