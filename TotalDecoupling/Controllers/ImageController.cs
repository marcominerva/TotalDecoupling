using Microsoft.AspNetCore.Mvc;
using TotalDecoupling.BusinessLayer.Services.Interfaces;

namespace TotalDecoupling.Controllers;

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
