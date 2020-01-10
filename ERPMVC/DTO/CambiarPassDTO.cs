using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CambiarPassDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordAnterior { get; set; }
    }
}
