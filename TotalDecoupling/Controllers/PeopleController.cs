using Microsoft.AspNetCore.Mvc;
using TotalDecoupling.BusinessLayer.Services.Interfaces;
using TotalDecoupling.Shared.Models;

namespace TotalDecoupling.Controllers;

public class PeopleController : ControllerBase
{
    private readonly IPeopleService peopleService;

    public PeopleController(IPeopleService peopleService)
    {
        this.peopleService = peopleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
        => CreateResponse(await peopleService.GetAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => CreateResponse(await peopleService.GetAsync(id));

    [HttpPost]
    public async Task<IActionResult> Save(Person person)
        => CreateResponse(await peopleService.SaveAsync(person));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => CreateResponse(await peopleService.DeleteAsync(id));
}
