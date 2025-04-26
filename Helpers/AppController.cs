using Azure;
using Microsoft.AspNetCore.Mvc;

namespace TodoAppApi.Helpers;

public class AppController : ControllerBase
{
    public async Task<IActionResult> Handle<T>(Func<Task<T>> action)
    {
        try
        {
            var response = await action();
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
}