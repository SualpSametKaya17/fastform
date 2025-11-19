using FastForm.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace FastForm.Services
{
    public class ExportService : IExportService
    {
        private readonly FastFormDbContext _context;
        private readonly IFormDataService _formDataService;

        public ExportService(FastFormDbContext context, IFormDataService formDataService)
        {
            _context = context;
            _formDataService = formDataService;
        }

        public async Task<string> ExportFormInstanceAsync(int instanceId, ExportFormat format, string outputPath)
        {
            try
            {
                var bytes = await ExportFormInstanceToByteArrayAsync(instanceId, format);

                var directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await File.WriteAllBytesAsync(outputPath, bytes);

                Log.Information("Form instance exported: {InstanceId} to {OutputPath}", instanceId, outputPath);
                return outputPath;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error exporting form instance: {InstanceId}", instanceId);
                throw;
            }
        }

        public async Task<byte[]> ExportFormInstanceToByteArrayAsync(int instanceId, ExportFormat format)
        {
            try
            {
                var instance = await _formDataService.GetInstanceByIdAsync(instanceId);
                if (instance == null)
                    throw new Exception($"Form instance not found: {instanceId}");

                var template = instance.FormTemplate;
                if (template == null || template.ImageData == null)
                    throw new Exception("Form template or image data not found");

                // TODO: Implement actual image processing with SkiaSharp or System.Drawing
                // For now, return the original template image without field overlays
                Log.Warning("Export feature is placeholder - returning template image without field values overlaid");
                Log.Information("To implement: Use SkiaSharp or System.Drawing to overlay field values on template");

                // Just return the template image for now
                return template.ImageData;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating export byte array for instance: {InstanceId}", instanceId);
                throw;
            }
        }

        public async Task<bool> BatchExportAsync(IEnumerable<int> instanceIds, ExportFormat format, string outputFolder)
        {
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                foreach (var instanceId in instanceIds)
                {
                    try
                    {
                        var instance = await _formDataService.GetInstanceByIdAsync(instanceId);
                        if (instance == null) continue;

                        var extension = format.ToString().ToLower();
                        var fileName = $"{instance.InstanceNumber}_{DateTime.Now:yyyyMMddHHmmss}.{extension}";
                        var outputPath = Path.Combine(outputFolder, fileName);

                        await ExportFormInstanceAsync(instanceId, format, outputPath);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error exporting instance in batch: {InstanceId}", instanceId);
                    }
                }

                Log.Information("Batch export completed to: {OutputFolder}", outputFolder);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in batch export");
                throw;
            }
        }
    }
}
