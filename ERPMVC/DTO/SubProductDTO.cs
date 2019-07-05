using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class SubProductDTO : SubProduct
    {
        public List<SubProduct> _SubProduct { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
