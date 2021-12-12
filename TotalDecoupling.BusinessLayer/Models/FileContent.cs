using System.IO;

namespace TotalDecoupling.BusinessLayer.Models;

public record StreamFileContent(Stream Content, string ContentType, string DownloadFileName = null);

public record ByteArrayFileContent(byte[] Content, string ContentType, string DownloadFileName = null);
