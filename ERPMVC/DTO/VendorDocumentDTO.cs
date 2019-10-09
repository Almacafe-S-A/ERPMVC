using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class VendorDocumentDTO: VendorDocument
    {
        public List<VendorDocument> _VendorDocument { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
