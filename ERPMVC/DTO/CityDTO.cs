using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CityDTO : City
    {
        public List<City> _City { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
