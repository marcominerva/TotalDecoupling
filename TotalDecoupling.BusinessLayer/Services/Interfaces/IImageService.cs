using TotalDecoupling.BusinessLayer.Models;

namespace TotalDecoupling.BusinessLayer.Services.Interfaces;

public interface IImageService
{
    Task<OperationResult<ByteArrayFileContent>> GetImageAsync();
}
