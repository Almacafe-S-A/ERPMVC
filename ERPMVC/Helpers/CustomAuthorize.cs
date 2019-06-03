using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERPMVC.Helpers
{


    [AttributeUsage(AttributeTargets.All)]  
    public sealed class CustomAuthorization : Attribute, IAuthorizationFilter
    {
      

        public  void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var controllerInfo = filterContext.ActionDescriptor as ControllerActionDescriptor;
            var ses = filterContext.HttpContext.Session.GetString("token");



            if (ses != null)
            {

                //string baseadress = "";
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + filterContext.HttpContext.Session.GetString("token"));
                //var result =  _client.GetAsync(baseadress + "api/customer");
                //string valorrespuesta = "";
                //if (result.co)
                //{
                //}

                var expira = Convert.ToDateTime(filterContext.HttpContext.Session.GetString("Expiration"));
                if (expira <= DateTime.Now)
                {
                    filterContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary {
                      {
                       "Controller",
                       "Account"
                      }, {
                       "Action",
                       "Login"
                      }
                   });
                }
                //filterContext.Result = new RedirectToRouteResult(
                // new RouteValueDictionary {
                //     {
                //      "Controller",
                //      "Home"
                //     }, {
                //      "Action",
                //      "Index"
                //     }
                // });

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary {
                      {
                       "Controller",
                       "Account"
                      }, {
                       "Action",
                       "Login"
                      }
                 });

            }
            //if (filterContext != null)
            //{
            //    string controllerName = controllerInfo.ControllerName;

            //    if (controllerName != "Home")
            //    {

            //        if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //        {
            //            filterContext.Result = new JsonResult("")
            //            {
            //                Value = new
            //                {
            //                    Status = "Error"
            //                },
            //            };
            //        }
            //        else
            //        {
            //            filterContext.Result = new RedirectToRouteResult(
            //             new RouteValueDictionary {
            //                  {
            //                   "Controller",
            //                   "Home"
            //                  }, {
            //                   "Action",
            //                   "SessionExpired"
            //                  }
            //             });
            //        }
            //    }
            //}




        }



    }




    //public class CustomAuthorize : AuthorizeAttribute
    //{
    //    private readonly PermissionAction[] permissionActions;

    //    public CustomAuthorize(PermissionItem item, params PermissionAction[] permissionActions)
    //    {
    //        this.permissionActions = permissionActions;
    //    }

    //    protected override Boolean IsAuthorized(HttpActionContext actionContext)
    //    {
    //        var currentIdentity = actionContext.RequestContext.Principal.Identity;
    //        if (!currentIdentity.IsAuthenticated)
    //            return false;

    //        var userName = currentIdentity.Name;
    //        using (var context = new DataContext())
    //        {
    //            var userStore = new UserStore<AppUser>(context);
    //            var userManager = new UserManager<AppUser>(userStore);
    //            var user = userManager.FindByName(userName);

    //            if (user == null)
    //                return false;

    //            foreach (var role in permissionActions)
    //                if (!userManager.IsInRole(user.Id, Convert.ToString(role)))
    //                    return false;

    //            return true;
    //        }
    //    }


    //}



}
