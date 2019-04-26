using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {

        public Guid UserId { get; set; }

        public Guid PolicyId { get; set; }
    }

    public class AspNetUserTokens : IdentityUserToken<Guid> { /*your code here*/ }
    public class AspNetRoleClaims : IdentityRoleClaim<Guid> { /*your code here*/ }
    public class AspNetUserLogins : IdentityUserLogin<Guid> { /*your code here*/ }


}
