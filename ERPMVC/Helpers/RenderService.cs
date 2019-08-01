using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPMVC.Helpers
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }

    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpContext _context;

        public ViewRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor accessor)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _context = accessor.HttpContext;
        }

        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(_context, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
                //  var viewEngineResult = _viewEngine.GetView(ViewContext.ExecutingFilePath, partialName, isMainPage: false);
                

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }


        public class ViewRender
        {
            private IRazorViewEngine _viewEngine;
            private ITempDataProvider _tempDataProvider;
            private IServiceProvider _serviceProvider;
            private readonly HttpContext _context;


            public ViewRender(
                IRazorViewEngine viewEngine,
                ITempDataProvider tempDataProvider,
                IServiceProvider serviceProvider,
                IHttpContextAccessor accessor)
            {
                _viewEngine = viewEngine;
                _tempDataProvider = tempDataProvider;
                _serviceProvider = serviceProvider;
                _context = accessor.HttpContext;
            }


            private string renderViewAsString<TModel>(string folder, string viewName, TModel model)
            {
                var actionContext = new ActionContext(_context, new RouteData(), new ActionDescriptor());
                var viewEngineResult = _viewEngine.FindView(actionContext, folder + "/" + viewName, false);
                var view = viewEngineResult.View;

                var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary());
                viewData.Model = model;

                var tempData = new TempDataDictionary(_context, _tempDataProvider);

                using (var output = new StringWriter())
                {
                    var viewContext = new ViewContext(actionContext, view, viewData, tempData, output, new HtmlHelperOptions());
                    viewContext.RouteData = _context.GetRouteData();   //set route data here

                    var task = view.RenderAsync(viewContext);
                    task.Wait();

                    return output.ToString();
                }
            }

            public string Render<TModel>(string name, TModel model)
            {
                var actionContext = new ActionContext(_context, new RouteData(), new ActionDescriptor());


                var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

                if (!viewEngineResult.Success)
                {
                    throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
                }

                var view = viewEngineResult.View;

                using (var output = new StringWriter())
                {
                    var viewContext = new ViewContext(
                        actionContext,
                        view,
                        new ViewDataDictionary<TModel>(
                            metadataProvider: new EmptyModelMetadataProvider(),
                            modelState: new ModelStateDictionary())
                        {
                            Model = model
                        },
                        new TempDataDictionary(
                            actionContext.HttpContext,
                            _tempDataProvider),
                        output,
                        new HtmlHelperOptions());

                    view.RenderAsync(viewContext).GetAwaiter().GetResult();

                    return output.ToString();
                }
            }

            private ActionContext GetActionContext()
            {
                var httpContext = new DefaultHttpContext();
                httpContext.RequestServices = _serviceProvider;
                return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            }
        }


 



    }


    public static class ViewResultExtensions
    {
        public static string ToHtml(this ViewResult result, HttpContext httpContext)
        {
            var feature = httpContext.Features.Get<IRoutingFeature>();
            var routeData = feature.RouteData;
            var viewName = result.ViewName ?? routeData.Values["action"] as string;
            var actionContext = new ActionContext(httpContext, routeData, new ControllerActionDescriptor());
            var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;
            var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, true) ?? options.Value.ViewEngines.Select(x => x.FindView(actionContext, viewName, true)).FirstOrDefault(x => x != null);
            var view = viewEngineResult.View;
            var builder = new StringBuilder();

            using (var output = new StringWriter(builder))
            {
                var viewContext = new ViewContext(actionContext, view, result.ViewData, result.TempData, output, htmlHelperOptions);

                view
                    .RenderAsync(viewContext)
                    .GetAwaiter()
                    .GetResult();
            }

            return builder.ToString();
        }
    }


}
