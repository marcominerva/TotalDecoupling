using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TotalDecoupling.BusinessLayer.Services;
using TotalDecoupling.Shared.Models;

namespace TotalDecoupling.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
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
}
