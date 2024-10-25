using ITEquipment.Interfaces;
using Microsoft.AspNetCore.Http.Features;

namespace ITEquipment.Api.Middleware.ErrorHandlingMiddlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ICommonMethods errorHandlingServices)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == 404)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Headers.Add("X-Redirect", "/ErrorHandling/PageNotFound");
                    var a = context.Features.Get<IHttpRequestFeature>();
                    if (a.RawTarget == "/signin-oidc")
                    {
                        context.Response.Redirect("/Subscription/Index");
                    }
                    else
                    {
                        context.Response.Redirect("/ErrorHandling/PageNotFound");
                    }
                }
            }
            catch (Exception ex)
            {
                await errorHandlingServices.HandleThrownErrorAsync(ex, context);
            }
        }
    }
}
