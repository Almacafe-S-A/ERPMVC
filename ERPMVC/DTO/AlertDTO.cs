using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class AlertDTO: Alert
    {
        public int editar { get; set; } = 1;
    }
}
