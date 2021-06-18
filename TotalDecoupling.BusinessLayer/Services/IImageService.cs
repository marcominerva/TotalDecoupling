using System.Threading.Tasks;
using TotalDecoupling.BusinessLayer.Models;

namespace TotalDecoupling.BusinessLayer.Services
{
    public interface IImageService
    {
        Task<OperationResult<ByteArrayFileContent>> GetImageAsync();
    }
}