using System.IO;
using System.Threading.Tasks;
using TotalDecoupling.BusinessLayer.Models;

namespace TotalDecoupling.BusinessLayer.Services;

public class ImageService : IImageService
{
    public async Task<OperationResult<ByteArrayFileContent>> GetImageAsync()
    {
        if (!File.Exists(@"D:\Image.jpg"))
        {
            return OperationResult.Fail(FailureReason.ItemNotFound);
        }

        var content = await File.ReadAllBytesAsync(@"D:\Image.jpg");
        return new ByteArrayFileContent(content, "image/jpg");
    }
}
