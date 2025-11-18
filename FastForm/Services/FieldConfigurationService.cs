using FastForm.Data;
using FastForm.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace FastForm.Services
{
    public class FieldConfigurationService : IFieldConfigurationService
    {
        private readonly FastFormDbContext _context;

        public FieldConfigurationService(FastFormDbContext context)
        {
            _context = context;
        }

        public async Task<string> ExportConfigurationAsync(int templateId)
        {
            try
            {
                var fields = await _context.FieldDefinitions
                    .Where(f => f.FormTemplateId == templateId)
                    .OrderBy(f => f.TabIndex)
                    .ToListAsync();

                var config = new
                {
                    Version = "1.0",
                    ExportDate = DateTime.Now,
                    TemplateId = templateId,
                    Fields = fields.Select(f => new
                    {
                        f.FieldName,
                        f.FieldLabel,
                        f.FieldType,
                        f.X,
                        f.Y,
                        f.Width,
                        f.Height,
                        f.FontFamily,
                        f.FontSize,
                        f.FontColor,
                        f.BackgroundColor,
                        f.BorderColor,
                        f.BorderThickness,
                        f.IsBold,
                        f.IsItalic,
                        f.IsUnderline,
                        f.TextAlignment,
                        f.VerticalAlignment,
                        f.IsRequired,
                        f.DefaultValue,
                        f.PlaceholderText,
                        f.ValidationRegex,
                        f.ValidationMessage,
                        f.MinLength,
                        f.MaxLength,
                        f.MinValue,
                        f.MaxValue,
                        f.FieldOptions,
                        f.TabIndex,
                        f.IsReadOnly,
                        f.IsVisible,
                        f.HelpText
                    })
                };

                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                Log.Information("Configuration exported for template: {TemplateId}", templateId);
                return json;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error exporting configuration for template: {TemplateId}", templateId);
                throw;
            }
        }

        public async Task<bool> ImportConfigurationAsync(int templateId, string configJson)
        {
            try
            {
                if (!await ValidateConfigurationAsync(configJson))
                {
                    Log.Warning("Invalid configuration JSON");
                    return false;
                }

                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                if (config == null) return false;

                // Remove existing field definitions
                var existingFields = await _context.FieldDefinitions
                    .Where(f => f.FormTemplateId == templateId)
                    .ToListAsync();

                _context.FieldDefinitions.RemoveRange(existingFields);

                // Add new field definitions
                var fields = DeserializeFieldDefinitions(configJson);
                foreach (var field in fields)
                {
                    field.FormTemplateId = templateId;
                    field.CreatedDate = DateTime.Now;
                    field.ModifiedDate = DateTime.Now;
                    _context.FieldDefinitions.Add(field);
                }

                await _context.SaveChangesAsync();

                Log.Information("Configuration imported for template: {TemplateId}", templateId);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error importing configuration for template: {TemplateId}", templateId);
                throw;
            }
        }

        public Task<bool> ValidateConfigurationAsync(string configJson)
        {
            try
            {
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                if (config == null) return Task.FromResult(false);

                // Basic validation
                if (config.Fields == null) return Task.FromResult(false);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error validating configuration");
                return Task.FromResult(false);
            }
        }

        public string SerializeFieldDefinitions(IEnumerable<FieldDefinition> fields)
        {
            try
            {
                var json = JsonConvert.SerializeObject(fields, Formatting.Indented);
                return json;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error serializing field definitions");
                throw;
            }
        }

        public IEnumerable<FieldDefinition> DeserializeFieldDefinitions(string json)
        {
            try
            {
                var config = JsonConvert.DeserializeObject<dynamic>(json);
                if (config == null || config.Fields == null)
                    return Enumerable.Empty<FieldDefinition>();

                var fields = new List<FieldDefinition>();
                foreach (var fieldData in config.Fields)
                {
                    var field = new FieldDefinition
                    {
                        FieldName = fieldData.FieldName,
                        FieldLabel = fieldData.FieldLabel,
                        FieldType = fieldData.FieldType,
                        X = fieldData.X,
                        Y = fieldData.Y,
                        Width = fieldData.Width,
                        Height = fieldData.Height,
                        FontFamily = fieldData.FontFamily ?? "Arial",
                        FontSize = fieldData.FontSize ?? 12,
                        FontColor = fieldData.FontColor ?? "#000000",
                        BackgroundColor = fieldData.BackgroundColor,
                        BorderColor = fieldData.BorderColor,
                        BorderThickness = fieldData.BorderThickness ?? 0,
                        IsBold = fieldData.IsBold ?? false,
                        IsItalic = fieldData.IsItalic ?? false,
                        IsUnderline = fieldData.IsUnderline ?? false,
                        TextAlignment = fieldData.TextAlignment ?? "Left",
                        VerticalAlignment = fieldData.VerticalAlignment ?? "Top",
                        IsRequired = fieldData.IsRequired ?? false,
                        DefaultValue = fieldData.DefaultValue,
                        PlaceholderText = fieldData.PlaceholderText,
                        ValidationRegex = fieldData.ValidationRegex,
                        ValidationMessage = fieldData.ValidationMessage,
                        MinLength = fieldData.MinLength,
                        MaxLength = fieldData.MaxLength,
                        MinValue = fieldData.MinValue,
                        MaxValue = fieldData.MaxValue,
                        FieldOptions = fieldData.FieldOptions,
                        TabIndex = fieldData.TabIndex ?? 0,
                        IsReadOnly = fieldData.IsReadOnly ?? false,
                        IsVisible = fieldData.IsVisible ?? true,
                        HelpText = fieldData.HelpText
                    };
                    fields.Add(field);
                }

                return fields;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deserializing field definitions");
                throw;
            }
        }
    }
}
