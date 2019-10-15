using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class KardexDTO : Kardex
    {
        public List<Int64> Ids { get; set; } = new List<long>();
    }


}
