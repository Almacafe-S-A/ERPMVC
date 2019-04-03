using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{

    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }
        public string Value { get; }
        public string Type { get; }

        public List<PolicyRoles> _policiesroles { get; set; }

        public HasScopeRequirement(string scope, string value,string type)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
           // Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
           Value = value ?? throw new ArgumentNullException(nameof(value));
           Type = type ?? throw new ArgumentNullException(nameof(type));

        }


        //public HasScopeRequirement(string scope,List<PolicyRoles> _policiesroles)
        //{
        //    Scope = scope ?? throw new ArgumentNullException(nameof(scope));


        //}





        public HasScopeRequirement(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }



    }



}
