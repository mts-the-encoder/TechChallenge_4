using Api.Filters;
using Application.UseCases.Movies.Create;
using Application.UseCases.Movies.Delete;
using Application.UseCases.Movies.GetAll;
using Application.UseCases.Movies.GetById;
using Application.UseCases.Movies.Update;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;
using HashidsModelBinder = Api.Binder.HashidsModelBinder;

namespace Api.Controllers;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class MovieController : TechChallengeController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromServices] ICreateMovieUseCase useCase, [FromBody] MovieRequest request)
    {
         var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromServices] IGetByMovieUseCase useCase, 
        [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll([FromServices] IGetAllMovieUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromServices] IUpdateMovieUseCase useCase,
        [FromBody] MovieRequest request,
        [FromRoute] long id)
    {
        var response = await useCase.Execute(id, request);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] IDeleteMovieUseCase useCase,
        [FromRoute] long id)
    {
        await useCase.Execute(id);

        return Ok();
    }
}