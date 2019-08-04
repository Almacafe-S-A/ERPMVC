using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC
{
    public abstract class MyRazorPage<T> : RazorPage<T>
    {
        public async Task<bool> HasPolicyAsync(string policyName)
        {

            try
            {
                var authorizationService = Context.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
                bool isAdmin = (await authorizationService.AuthorizeAsync(User, policyName)).Succeeded;
                return isAdmin;
            }
            catch (Exception ex)
            {

                return false;
            }
         
        }
    }


    public class Politicas
    {
        public const string GG = "GG";
        public const string Admin = "Admin";


    }

    //public enum Politicas {
    //      GG ,
    //      Admin,
          

    //}



}
