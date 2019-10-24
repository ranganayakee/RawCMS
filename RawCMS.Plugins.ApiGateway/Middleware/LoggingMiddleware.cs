﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawCMS.Library.Core.Extension;
using RawCMS.Plugins.ApiGateway.Classes;
using RawCMS.Plugins.ApiGateway.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RawCMS.Plugins.ApiGateway.Middleware
{
    [MiddlewarePriority(Order = 3)]
    public class LoggingMiddleware : GatewayMiddleware
    {
        public LoggingMiddleware(RequestDelegate requestDelegate, ILogger logger, ApiGatewayConfig config, IEnumerable<RawHandler> handlers)
            : base(requestDelegate, logger, config, handlers)
        {
        }

        public override string Name => "LoggingMiddleware";

        public override string Description => "Enable Logging capability";

        public async override Task InvokeAsync(HttpContext context)
        {
            await next(context);
            try
            {
                logger.LogInformation($"Request: {context.Request.Path}");
                logger.LogInformation($"Method: {context.Request.Method}");
                logger.LogInformation($"Headers: {JsonConvert.SerializeObject(context.Request.Headers)}");
                if (context.Request.Body != null && context.Request.Body.CanRead)
                {
                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        logger.LogInformation($"Content: {reader.ReadToEndAsync()}");
                    }
                }
                logger.LogInformation("Response************");
                logger.LogInformation($"Status Code: {context.Response.StatusCode}");
                logger.LogInformation($"Headers: {JsonConvert.SerializeObject(context.Response.Headers)}");

                if (context.Request.Body != null && context.Request.Body.CanRead)
                {
                    using (var reader = new StreamReader(context.Response.Body))
                    {
                        logger.LogInformation($"Content: {reader.ReadToEndAsync()}");
                    }
                }
            }
            catch { }
        }
    }
}
