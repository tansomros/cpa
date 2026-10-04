using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.NewsArticles.Commands.Create;
using BigLion.CPA.Application.Features.NewsArticles.Commands.Delete;
using BigLion.CPA.Application.Features.NewsArticles.Commands.Update;
using BigLion.CPA.Application.Features.NewsArticles.Queries.Get;
using BigLion.CPA.Application.Features.NewsArticles.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class NewsArticlesController : BaseController
{
    [HttpPost(Name = "CreateNews")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateNewsCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetNews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<NewsViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetNewsQuery { NewsID = id }));
    }

    [HttpGet(Name = "GetNewsList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<NewsViewModel>))]
    public async Task<ActionResult<PaginatedList<NewsViewModel>>> List([FromQuery] GetNewsListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateNews")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateNewsCommand command)
    {
        if (id != command.NewsID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteNews")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteNewsCommand { NewsID = id });
        return NoContent();
    }
}
