using System.Text;
using FabriqPro.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FabriqPro.Core;

public class CoreExceptionFilters : IExceptionFilter
{
  public void OnException(ExceptionContext context)
  {
    var message = new StringBuilder();
    var statusCode = 400;
    switch (context.Exception)
    {
      case AlreadyExistsException exc:
        message.Append($"You are entering a duplicate value for a unique column. {exc.Message}");
        statusCode = 409;
        context.Result = new ObjectResult(message.ToString()) { StatusCode = statusCode };
        break;
      case DoesNotExistException exc:
        message.Append($"Object with the given credentials does not exist. {exc.Message}");
        statusCode = 404;
        context.Result = new ObjectResult(message.ToString()) { StatusCode = statusCode };
        break;
    }
  }
}