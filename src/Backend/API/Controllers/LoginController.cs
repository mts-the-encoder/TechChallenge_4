using Application.UseCases.Login.DoLogin;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class LoginController : TechChallengeController
{
    [HttpPost]
    public async Task<IActionResult> Login([FromServices] ILoginUseCase useCase,[FromBody]LoginRequest request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}