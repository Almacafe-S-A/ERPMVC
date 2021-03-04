using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ClaimsDTO
    {
        public string Tipo { get; set; }
        public string Nombre { get; set; }
    }

    class CustomClaim : Claim
    {
        public CustomClaim(string type, string value, string valueType, string issuer, string originalIssuer) :
            base(type, value, valueType, issuer, originalIssuer)
        { }
    }
}
