using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ERPMVC.DTO
{
    public class EmployeesDTO : Employees
    {
        public List<Employees> _Employees { get; set; }
        public decimal QtySalary { get; set; }
        public DateTime DayApplication { get; set; }
        public string GeneroName { get; set; }
        public string TipoSangreName { get; set; }


        public int editar { get; set; } = 1;

        public string token { get; set; }
        public IFormFile files { get; set; }
    }
}
