using FastForm.Data;
using FastForm.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FastForm.Services
{
    public class FormTemplateService : IFormTemplateService
    {
        private readonly FastFormDbContext _context;

        public FormTemplateService(FastFormDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormTemplate>> GetAllTemplatesAsync(bool includeInactive = false)
        {
            try
            {
                var query = _context.FormTemplates
                    .Include(t => t.FieldDefinitions)
                    .Include(t => t.CreatedBy)
                    .AsQueryable();

                if (!includeInactive)
                {
                    query = query.Where(t => t.IsActive);
                }

                return await query.OrderByDescending(t => t.CreatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting form templates");
                throw;
            }
        }

        public async Task<FormTemplate?> GetTemplateByIdAsync(int id)
        {
            try
            {
                return await _context.FormTemplates
                    .Include(t => t.FieldDefinitions)
                    .Include(t => t.CreatedBy)
                    .Include(t => t.ModifiedBy)
                    .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting form template by id: {Id}", id);
                throw;
            }
        }

        public async Task<FormTemplate> CreateTemplateAsync(FormTemplate template)
        {
            try
            {
                template.CreatedDate = DateTime.Now;
                template.ModifiedDate = DateTime.Now;

                _context.FormTemplates.Add(template);
                await _context.SaveChangesAsync();

                Log.Information("Form template created: {Name} (Id: {Id})", template.Name, template.Id);
                return template;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating form template: {Name}", template.Name);
                throw;
            }
        }

        public async Task<FormTemplate> UpdateTemplateAsync(FormTemplate template)
        {
            try
            {
                template.ModifiedDate = DateTime.Now;

                _context.FormTemplates.Update(template);
                await _context.SaveChangesAsync();

                Log.Information("Form template updated: {Name} (Id: {Id})", template.Name, template.Id);
                return template;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating form template: {Id}", template.Id);
                throw;
            }
        }

        public async Task<bool> DeleteTemplateAsync(int id)
        {
            try
            {
                var template = await _context.FormTemplates.FindAsync(id);
                if (template == null) return false;

                _context.FormTemplates.Remove(template);
                await _context.SaveChangesAsync();

                Log.Information("Form template deleted: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting form template: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<FieldDefinition>> GetFieldDefinitionsAsync(int templateId)
        {
            try
            {
                return await _context.FieldDefinitions
                    .Where(f => f.FormTemplateId == templateId)
                    .OrderBy(f => f.TabIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting field definitions for template: {TemplateId}", templateId);
                throw;
            }
        }

        public async Task<FieldDefinition> AddFieldDefinitionAsync(FieldDefinition field)
        {
            try
            {
                field.CreatedDate = DateTime.Now;
                field.ModifiedDate = DateTime.Now;

                _context.FieldDefinitions.Add(field);
                await _context.SaveChangesAsync();

                Log.Information("Field definition added: {FieldName} (Id: {Id})", field.FieldName, field.Id);
                return field;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding field definition: {FieldName}", field.FieldName);
                throw;
            }
        }

        public async Task<FieldDefinition> UpdateFieldDefinitionAsync(FieldDefinition field)
        {
            try
            {
                field.ModifiedDate = DateTime.Now;

                _context.FieldDefinitions.Update(field);
                await _context.SaveChangesAsync();

                Log.Information("Field definition updated: {FieldName} (Id: {Id})", field.FieldName, field.Id);
                return field;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating field definition: {Id}", field.Id);
                throw;
            }
        }

        public async Task<bool> DeleteFieldDefinitionAsync(int fieldId)
        {
            try
            {
                var field = await _context.FieldDefinitions.FindAsync(fieldId);
                if (field == null) return false;

                _context.FieldDefinitions.Remove(field);
                await _context.SaveChangesAsync();

                Log.Information("Field definition deleted: {Id}", fieldId);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting field definition: {Id}", fieldId);
                throw;
            }
        }

        public async Task<bool> SaveConfigurationAsync(int templateId, string configJson, string? description = null)
        {
            try
            {
                var history = new ConfigurationHistory
                {
                    FormTemplateId = templateId,
                    ConfigurationJson = configJson,
                    ChangeDescription = description,
                    ChangedDate = DateTime.Now
                };

                _context.ConfigurationHistories.Add(history);
                await _context.SaveChangesAsync();

                Log.Information("Configuration saved for template: {TemplateId}", templateId);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving configuration for template: {TemplateId}", templateId);
                throw;
            }
        }

        public async Task<IEnumerable<ConfigurationHistory>> GetConfigurationHistoryAsync(int templateId)
        {
            try
            {
                return await _context.ConfigurationHistories
                    .Where(h => h.FormTemplateId == templateId)
                    .Include(h => h.ChangedBy)
                    .OrderByDescending(h => h.ChangedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting configuration history for template: {TemplateId}", templateId);
                throw;
            }
        }
    }
}
