using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERPMVC
{
         [HtmlTargetElement(Attributes = "policy")]
        public class PolicyTagHelper : TagHelper
        {
            private readonly IAuthorizationService _authService;
            private readonly ClaimsPrincipal _principal;

            public PolicyTagHelper(IAuthorizationService authService, IHttpContextAccessor httpContextAccessor)
            {
                _authService = authService;
                _principal = httpContextAccessor.HttpContext.User;
            }

            public string Policy { get; set; }

            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                // if (!await _authService.AuthorizeAsync(_principal, Policy)) ASP.NET Core 1.x
                if (!(await _authService.AuthorizeAsync(_principal, Policy)).Succeeded)
                    output.SuppressOutput();

                  
            }
        }



    [HtmlTargetElement(Attributes = "deshabilitar")]
    public class DeshabilitarTagHelper : TagHelper
    {
        private readonly IAuthorizationService _authService;
        private readonly ClaimsPrincipal _principal;

        public DeshabilitarTagHelper(IAuthorizationService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public string deshabilitar { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // if (!await _authService.AuthorizeAsync(_principal, Policy)) ASP.NET Core 1.x
           // if(Policy!=null)
            if (!(await _authService.AuthorizeAsync(_principal, deshabilitar)).Succeeded)
               output.Attributes.SetAttribute("disabled","disabled");
               output.Attributes.SetAttribute("class","noclick");
           

        }
    }




    //[HtmlTargetElement("textarea", Attributes = ForAttributeName)]
    //public class MyCustomTextArea : TextAreaTagHelper
    //{
    //    private const string ForAttributeName = "asp-for";

    //    [HtmlAttributeName("asp-is-disabled")]
    //    public bool IsDisabled { set; get; }

    //    public MyCustomTextArea(IHtmlGenerator generator) : base(generator)
    //    {
    //    }

    //    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    //    {
    //        if (IsDisabled)
    //        {
    //            output.Attributes.SetAttribute("disabled","disabled");
    //        }

    //        base.Process(context, output);
    //    }


    //}







}
