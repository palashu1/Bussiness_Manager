using static System.Collections.Specialized.BitVector32;

namespace Bussiness_Manager.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 401)
                {
                    
                        context.Response.Redirect("/Home/Login?AddShop=" + context.Request.Path);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, etc.)
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}
