using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Helpers
{

    public class MyViewModel
    {
        public DateTime MyDate { get; set; }
    }

    public class MyViewModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(MyViewModel))
                return new MyViewModelBinder();

            return null;
        }
    }






    public class MyViewModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var day = 0;
            var month = 0;
            var year = 0;

            if (!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["day"], out day) ||
            !int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["month"], out month) ||
            !int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["year"], out year))
            {
                return Task.CompletedTask;
            }

            var result = new MyViewModel
            {
                MyDate = new DateTime(year, month, day)
            };

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }


    }
}
