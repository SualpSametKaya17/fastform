using FastForm.Data;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
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

                // Load base image
                using var baseImage = Image.Load<Rgba32>(template.ImageData);

                // Draw field values on image
                foreach (var fieldValue in instance.FieldValues)
                {
                    var field = fieldValue.FieldDefinition;
                    if (!field.IsVisible || string.IsNullOrEmpty(fieldValue.Value))
                        continue;

                    try
                    {
                        // Parse font properties
                        var fontSize = (float)field.FontSize;
                        var fontFamily = field.FontFamily;

                        // For now, use a basic system font
                        // In production, you'd load custom fonts
                        var font = SystemFonts.CreateFont(fontFamily, fontSize,
                            field.IsBold ? FontStyle.Bold :
                            field.IsItalic ? FontStyle.Italic :
                            FontStyle.Regular);

                        var color = ParseColor(field.FontColor);
                        var textOptions = new RichTextOptions(font)
                        {
                            Origin = new PointF((float)field.X, (float)field.Y),
                            HorizontalAlignment = ParseHorizontalAlignment(field.TextAlignment),
                            VerticalAlignment = ParseVerticalAlignment(field.VerticalAlignment)
                        };

                        baseImage.Mutate(ctx => ctx.DrawText(textOptions, fieldValue.Value, color));
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Error drawing field: {FieldName}", field.FieldName);
                    }
                }

                // Convert to requested format
                using var memoryStream = new MemoryStream();
                switch (format)
                {
                    case ExportFormat.PNG:
                        await baseImage.SaveAsPngAsync(memoryStream);
                        break;
                    case ExportFormat.JPEG:
                        await baseImage.SaveAsJpegAsync(memoryStream);
                        break;
                    case ExportFormat.PDF:
                        // For PDF, we'd use a library like QuestPDF or PdfSharp
                        // For now, export as PNG (simplified)
                        await baseImage.SaveAsPngAsync(memoryStream);
                        break;
                    case ExportFormat.DOCX:
                        // For DOCX, we'd use a library like DocX
                        // For now, export as PNG (simplified)
                        await baseImage.SaveAsPngAsync(memoryStream);
                        break;
                }

                return memoryStream.ToArray();
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

        private static Color ParseColor(string colorString)
        {
            try
            {
                if (colorString.StartsWith("#"))
                {
                    return Color.ParseHex(colorString);
                }
                return Color.Black;
            }
            catch
            {
                return Color.Black;
            }
        }

        private static HorizontalAlignment ParseHorizontalAlignment(string alignment)
        {
            return alignment?.ToLower() switch
            {
                "center" => HorizontalAlignment.Center,
                "right" => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Left
            };
        }

        private static VerticalAlignment ParseVerticalAlignment(string alignment)
        {
            return alignment?.ToLower() switch
            {
                "middle" => VerticalAlignment.Center,
                "bottom" => VerticalAlignment.Bottom,
                _ => VerticalAlignment.Top
            };
        }
    }
}
