using FastForm.Models;

namespace FastForm.Services
{
    public interface IFieldConfigurationService
    {
        Task<string> ExportConfigurationAsync(int templateId);
        Task<bool> ImportConfigurationAsync(int templateId, string configJson);
        Task<bool> ValidateConfigurationAsync(string configJson);
        string SerializeFieldDefinitions(IEnumerable<FieldDefinition> fields);
        IEnumerable<FieldDefinition> DeserializeFieldDefinitions(string json);
    }
}
