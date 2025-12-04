

using BlogApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{

    public AuthController()
    {

    }

}