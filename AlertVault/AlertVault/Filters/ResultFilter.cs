using AlertVault.Core.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlertVault.Filters;

public class ResultFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult objectResult)
        {
            return;
        }

        context.Result = objectResult.Value switch
        {
            not null when IsResultOfT(objectResult.Value, out var success, out var value, out var errors) => 
                success
                    ? new OkObjectResult(value)
                    : new BadRequestObjectResult(new { errors }),
            Result { IsSuccess: true } => new NoContentResult(),
            Result { IsSuccess: false } result => new BadRequestObjectResult(new { result.Errors }),
            _ => context.Result
        };
    }

    private static bool IsResultOfT(object obj, out bool isSuccess, out object? value, out List<string> errors)
    {
        var type = obj.GetType();
        
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            isSuccess = (bool)type.GetProperty("IsSuccess")!.GetValue(obj)!;
            value = type.GetProperty("Value")!.GetValue(obj);
            errors = (List<string>)type.GetProperty("Errors")!.GetValue(obj)!;
            return true;
        }

        isSuccess = false;
        value = null;
        errors = [];
        return false;
    }
}