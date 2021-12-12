using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TotalDecoupling.BusinessLayer.Services;

namespace TotalDecoupling.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ImageController : ControllerBase
{
    private readonly IImageService imageService;

    public ImageController(IImageService imageService)
    {
        this.imageService = imageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetImage()
        => CreateResponse(await imageService.GetImageAsync());
}
