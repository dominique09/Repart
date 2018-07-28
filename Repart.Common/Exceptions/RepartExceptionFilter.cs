using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Repart.Common.Exceptions
{
    public class RepartExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != typeof(RepartException)) return;
            
            var ex = (RepartException)context.Exception;
            
            context.Result = new ObjectResult(
                new ErrorResponse()
            {
                Code = ex.Code,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            })
            {
                StatusCode = 500,
                DeclaredType = typeof(ErrorResponse)
            };
        }
    }

    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
