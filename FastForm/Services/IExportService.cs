using FastForm.Models;

namespace FastForm.Services
{
    public enum ExportFormat
    {
        PDF,
        PNG,
        JPEG,
        DOCX
    }

    public interface IExportService
    {
        Task<string> ExportFormInstanceAsync(int instanceId, ExportFormat format, string outputPath);
        Task<byte[]> ExportFormInstanceToByteArrayAsync(int instanceId, ExportFormat format);
        Task<bool> BatchExportAsync(IEnumerable<int> instanceIds, ExportFormat format, string outputFolder);
    }
}
