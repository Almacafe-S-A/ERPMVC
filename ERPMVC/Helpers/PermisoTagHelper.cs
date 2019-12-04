using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace ERPMVC.Helpers
{
    [HtmlTargetElement(Attributes = "tipo-permiso,permiso")]
    [HtmlTargetElement(Attributes = "tipo-permiso")]
    [HtmlTargetElement(Attributes = "permiso")]
    public class PermisoTagHelper : TagHelper
    {
        private readonly ClaimsPrincipal _principal;
        private readonly ILogger _logger;

        public PermisoTagHelper(ILogger<PermisoTagHelper> logger
            , IHttpContextAccessor httpContextAccessor)
        {
            _principal = httpContextAccessor.HttpContext.User;
            this._logger = logger;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            
            TagHelperAttribute permiso;
            if (!context.AllAttributes.TryGetAttribute("permiso", out permiso))
            {
                output.SuppressOutput();
                return Task.CompletedTask;
            }

            if(!_principal.HasClaim("Permiso",permiso.Value.ToString()))
                output.SuppressOutput();
            return Task.CompletedTask;
        }
    }
}
