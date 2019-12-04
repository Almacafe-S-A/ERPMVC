using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ERPMVC.Models
{
    public class UserToken
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public Int64 BranchId { get; set; }

        [Display(Name = "Habilitado")]
        public bool? IsEnabled { get; set; }

        public Int32 Passworddias { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
