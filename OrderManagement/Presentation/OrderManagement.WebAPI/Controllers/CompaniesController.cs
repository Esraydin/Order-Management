using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Queries.CompanyQueries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrderManagement.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var values = await _mediator.Send(new GetCompanyQuery());
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var value = await _mediator.Send(new GetCompanyByIdQuery(id));
        return Ok(value);
    }
   
    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyCommand command)
    {
        var value = await _mediator.Send(command);
        return Ok(value);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCompanyCommand command)
    {
        
            await _mediator.Send(command);
            return Ok();
        
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        await _mediator.Send(new RemoveCompanyCommand(id));
        return Ok();
    }
}
