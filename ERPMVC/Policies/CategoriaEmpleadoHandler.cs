using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Policies
{
    public class CategoriaEmpleadoHandler : AuthorizationHandler<CategoriaEmpleadoRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CategoriaEmpleadoRequirement requirement)
        {
            //Implementar la logica para saber si el usuario cumple o no el requerimiento(CategoriaEmpleadoRequirement)
            if (context.User.Claims.Any(x => x.Type == "CategoriaEmpleado" && x.Value=="3"))
            {
                context.Succeed(requirement);
            }

          return Task.FromResult(0);

        }





    }



    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }


    }


    public class UsuarioHandler : AuthorizationHandler<UsuarioRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UsuarioRequirement requirement)
        {
            if (context.User.IsInRole("GG"))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }


    }


}
