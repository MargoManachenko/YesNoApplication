using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace YesNoPuzzle.WebSocket
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class WebSocket
    {
        private readonly RequestDelegate _next;

        public WebSocket(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder UseWebSocket(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocket>();
        }
    }
}
