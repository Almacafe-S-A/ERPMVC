using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class Boleto_EntDTO: Boleto_Ent
    {
        public Int64 ProductId { get; set; }

        public Int64 UnitOfMeasureId { get; set; }
    }
}
