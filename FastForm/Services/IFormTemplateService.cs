using FastForm.Models;

namespace FastForm.Services
{
    public interface IFormTemplateService
    {
        Task<IEnumerable<FormTemplate>> GetAllTemplatesAsync(bool includeInactive = false);
        Task<FormTemplate?> GetTemplateByIdAsync(int id);
        Task<FormTemplate> CreateTemplateAsync(FormTemplate template);
        Task<FormTemplate> UpdateTemplateAsync(FormTemplate template);
        Task<bool> DeleteTemplateAsync(int id);
        Task<IEnumerable<FieldDefinition>> GetFieldDefinitionsAsync(int templateId);
        Task<FieldDefinition> AddFieldDefinitionAsync(FieldDefinition field);
        Task<FieldDefinition> UpdateFieldDefinitionAsync(FieldDefinition field);
        Task<bool> DeleteFieldDefinitionAsync(int fieldId);
        Task<bool> SaveConfigurationAsync(int templateId, string configJson, string? description = null);
        Task<IEnumerable<ConfigurationHistory>> GetConfigurationHistoryAsync(int templateId);
    }
}
