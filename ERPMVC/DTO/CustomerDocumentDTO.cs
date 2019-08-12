using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CustomerDocumentDTO : CustomerDocument
    {
        public IEnumerable<IFormFile> files { get; set; }

    }
}
