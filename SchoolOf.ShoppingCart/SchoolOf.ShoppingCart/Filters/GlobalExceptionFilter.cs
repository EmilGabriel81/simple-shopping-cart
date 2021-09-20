using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SchoolOf.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SchoolOf.ShoppingCart.Filters
{
   
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var guid = Guid.NewGuid().ToString();
            _logger.LogError(context.Exception, guid);
            context.ExceptionHandled = true;


            if (context.Exception is InvalidParameterException)
            {
                var ex = (InvalidParameterException)context.Exception;
                var response = new ErrorDto
                {
                    Errors = new List<String> { ex.Message }
                };
                context.Result = new JsonResult(response)
                {
                    StatusCode = 400
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is InternalValidationException internalValidationException)
            {
                var response = new ErrorDto
                {
                    Errors = internalValidationException.Errors
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = 400
                };
            }
            else
            {    
                var response = new ErrorDto
                {
                    Errors = new List<String> { "Something went wrong. Please contact the support team. Id: " +  guid }
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
    public class ErrorDto
    {
        public List<String> Errors { get; set; }
        
    }
}

