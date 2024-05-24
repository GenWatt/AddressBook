using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            Console.WriteLine("Error: " + statusCode);
            switch (statusCode)
            {
                case 404:
                    return View("NotFound");

                case 401:
                    return Redirect("/Identity/Account/Login");

                case 429:
                    return View("TooManyRequests");

                default:
                    return View("ServerError");
            }
        }

        public IActionResult Index()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                // Log your exception here
                _logger.LogError($"The path {routeWhereExceptionOccurred} threw an exception: {exceptionThatOccurred}");
            }

            return View("ServerError");
        }
    }
}