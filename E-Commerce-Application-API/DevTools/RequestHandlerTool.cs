using Microsoft.AspNetCore.Mvc;

namespace PokemonReviewApp.DevTools
{
    /// <summary>
    /// A class for that provides methods and tools for 
    /// handling different API problems.
    /// </summary>
    public class RequestHandlerTool
    {
        /// <summary>
        /// Handles the internal server error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="method">The method that was occured from.</param>
        /// <param name="filepath">The filepath that was occured from.</param>
        /// <param name="extraInfo">Extra information for the feedback.</param>
        /// <returns></returns>
        public static IActionResult HandleInternalServerError(Exception error, string method, string filepath, string extraInfo = "")
        {
            Console.WriteLine("Error occured at " + method.ToUpper() + " : " + filepath + " :  " + error.Message + "\n" + extraInfo);
            return new ObjectResult("Internal server error occurred while processing the request.\n" + extraInfo)
            {
                StatusCode = 500
            };
        }
    }
}
