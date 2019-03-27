using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Policies
{
    //Esta clase es para pasarle parametros al Handler
    public class CategoriaEmpleadoRequirement :IAuthorizationRequirement
    {    

    }

    public class AdminRequirement : IAuthorizationRequirement
    {

    }

}
