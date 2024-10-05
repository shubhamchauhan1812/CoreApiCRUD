using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace CoreApiCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly OpenAIAPI _openai;
        public OpenAIController()
        {
            _openai = new OpenAIAPI("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");
        }
        [HttpGet("UseChatGPT")]
        [Consumes("text/plain")]
        public async Task<IActionResult> UseChatGPT(string query)
        {
            //try { 

            //if(string.IsNullOrEmpty(query))
            //{
            //    return BadRequest(" Request can not be null.");
            //}
            //string outPutResult = "";
            //var openai = new OpenAIAPI("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");
            //CompletionRequest completionRequest = new CompletionRequest();
            //completionRequest.Prompt = query;
            //completionRequest.Model = OpenAI_API.Models.Model.DavinciText;

            //    OpenAI_API.OpenAIAPI openai = new OpenAI_API.OpenAIAPI("your-api-key");


            //    var completions = await openai.Completions.CreateCompletionsAsync(completionRequest);

            //foreach (var completion in completions.Completions)
            //{
            //    outPutResult += completion.Text;
            //}
            //return Ok(outPutResult);
            //}
            //catch(Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                string outputResult = "";

                // Check for empty query
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest("Query cannot be null or empty");
                }

                // Create the completion request
                var completionRequest = new CompletionRequest
                {
                    Prompt = query,
                    Model = "gpt-3.5-turbo", // Use the model as a string
                    MaxTokens = 100 // Adjust as needed
                };

                // Await the API call
                var completions = await _openai.Completions.CreateCompletionsAsync(completionRequest);

                // Check if completions is null
                if (completions == null || completions.Completions == null)
                {
                    return StatusCode(500, "The OpenAI API returned a null response.");
                }

                // Process the result
                foreach (var completion in completions.Completions)
                {
                    outputResult += completion.Text;
                }

                return Ok(outputResult);
            }
            catch (HttpRequestException httpEx)
            {
                // Handle potential network issues
                return StatusCode(500, $"Network error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Log or return the error message for other exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
