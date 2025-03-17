using Microsoft.AspNetCore.Mvc;

namespace Safety.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            // Get All Errors in Model State
            var errors = context.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidationError
                {
                    Field = error.Key,
                    Errors = error.Value.Errors.Select(e => e.ErrorMessage)
                });

            // Create Custom Response
            var response = new ValidationErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = $"Validation Failed",
                Errors = errors
            };

            // Return
            return new BadRequestObjectResult(response);
        }
    }
}
