using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class EmployeeDocumentDTO : EmployeeDocument
    {
        public IEnumerable<IFormFile> files { get; set; }

    }
}
