using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ERPMVC.DTO
{
    public class ConciliacionDTO : Conciliacion
    {
        public ConciliacionDTO()
        {
        }

        public string NombreCuenta { get; set; }
        public int Editar { get; set; }
    }
}
