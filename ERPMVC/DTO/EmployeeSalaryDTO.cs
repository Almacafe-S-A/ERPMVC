using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Models;

namespace ERPMVC.DTO
{
    public class EmployeeSalaryDTO : EmployeeSalary
    {
        public List<EmployeeSalary> _EmployeeSalary { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
