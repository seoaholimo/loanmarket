using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Helpers
{
    public class ViewRender
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRender(IRazorViewEngine viewEngine,ITempDataProvider tempDataProvider,IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public string Render<TModel>(string name, TModel model)
        {
            ActionContext actionContext = GetActionContext();

            ViewEngineResult viewEngineResult = _viewEngine.FindView(actionContext, name, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view '{name}'");
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(actionContext, view,
                    new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
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
            var httpContext = new DefaultHttpContext {RequestServices = _serviceProvider};
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}