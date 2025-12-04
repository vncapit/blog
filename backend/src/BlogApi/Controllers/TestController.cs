using Microsoft.AspNetCore.Mvc;
using BlogApi.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using BlogApi.Dtos.Common;
using System.Net;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController : ControllerBase
{
    private readonly AppDbContext _context;
    public TestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ping")]
    public ActionResult<ApiResponse<string>> Ping()
    {
        return Ok(ApiResponse<string>.Ok("pong"));
    }

    [HttpGet("ping/db")]
    public async Task<ActionResult> PingDb()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            return canConnect ? Ok(ApiResponse<string>.Ok("DB connected successfully!")) : StatusCode((int)HttpStatusCode.InternalServerError, ApiResponse<string>.Fail("Cannot connect to DB!"));
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ApiResponse<string>.Fail("Cannot connect to DB!", ex.Message));
        }
    }
}