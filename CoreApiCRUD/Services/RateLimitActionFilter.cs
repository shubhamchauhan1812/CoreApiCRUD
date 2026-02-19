
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
namespace CoreApiCRUD.Services
{
    public class RateLimitActionFilter : IActionFilter
    {
        private readonly RateLimitService _rateLimitService;

        public RateLimitActionFilter(RateLimitService rateLimitService)
        {
            _rateLimitService = rateLimitService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var clientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(clientIp) || !_rateLimitService.IsRequestAllowed(clientIp))
            {
                // If the IP is blocked or the rate limit is exceeded, respond with 429 Too Many Requests
                context.Result = new StatusCodeResult(429);  // Too Many Requests
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No actions after the request has been executed
        }
    }

}
