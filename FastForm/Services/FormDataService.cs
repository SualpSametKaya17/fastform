using FastForm.Data;
using FastForm.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FastForm.Services
{
    public class FormDataService : IFormDataService
    {
        private readonly FastFormDbContext _context;

        public FormDataService(FastFormDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormInstance>> GetAllInstancesAsync(int? templateId = null, string? status = null)
        {
            try
            {
                var query = _context.FormInstances
                    .Include(i => i.FormTemplate)
                    .Include(i => i.CreatedBy)
                    .Include(i => i.AssignedTo)
                    .Include(i => i.FieldValues)
                    .AsQueryable();

                if (templateId.HasValue)
                    query = query.Where(i => i.FormTemplateId == templateId.Value);

                if (!string.IsNullOrEmpty(status))
                    query = query.Where(i => i.Status == status);

                return await query.OrderByDescending(i => i.CreatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting form instances");
                throw;
            }
        }

        public async Task<FormInstance?> GetInstanceByIdAsync(int id)
        {
            try
            {
                return await _context.FormInstances
                    .Include(i => i.FormTemplate)
                        .ThenInclude(t => t.FieldDefinitions)
                    .Include(i => i.FieldValues)
                        .ThenInclude(v => v.FieldDefinition)
                    .Include(i => i.CreatedBy)
                    .Include(i => i.AssignedTo)
                    .Include(i => i.ApprovedBy)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting form instance by id: {Id}", id);
                throw;
            }
        }

        public async Task<FormInstance> CreateInstanceAsync(FormInstance instance)
        {
            try
            {
                instance.CreatedDate = DateTime.Now;
                instance.ModifiedDate = DateTime.Now;

                if (string.IsNullOrEmpty(instance.InstanceNumber))
                {
                    instance.InstanceNumber = await GenerateInstanceNumberAsync();
                }

                _context.FormInstances.Add(instance);
                await _context.SaveChangesAsync();

                Log.Information("Form instance created: {InstanceNumber} (Id: {Id})", instance.InstanceNumber, instance.Id);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating form instance");
                throw;
            }
        }

        public async Task<FormInstance> UpdateInstanceAsync(FormInstance instance)
        {
            try
            {
                instance.ModifiedDate = DateTime.Now;

                _context.FormInstances.Update(instance);
                await _context.SaveChangesAsync();

                Log.Information("Form instance updated: {InstanceNumber} (Id: {Id})", instance.InstanceNumber, instance.Id);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating form instance: {Id}", instance.Id);
                throw;
            }
        }

        public async Task<bool> DeleteInstanceAsync(int id)
        {
            try
            {
                var instance = await _context.FormInstances.FindAsync(id);
                if (instance == null) return false;

                _context.FormInstances.Remove(instance);
                await _context.SaveChangesAsync();

                Log.Information("Form instance deleted: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting form instance: {Id}", id);
                throw;
            }
        }

        public async Task<bool> SetFieldValueAsync(int instanceId, int fieldDefinitionId, string? value, byte[]? binaryData = null)
        {
            try
            {
                var fieldValue = await _context.FieldValues
                    .FirstOrDefaultAsync(v => v.FormInstanceId == instanceId && v.FieldDefinitionId == fieldDefinitionId);

                if (fieldValue == null)
                {
                    fieldValue = new FieldValue
                    {
                        FormInstanceId = instanceId,
                        FieldDefinitionId = fieldDefinitionId,
                        FieldValue_ = value,
                        BinaryData = binaryData,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    _context.FieldValues.Add(fieldValue);
                }
                else
                {
                    fieldValue.FieldValue_ = value;
                    fieldValue.BinaryData = binaryData;
                    fieldValue.ModifiedDate = DateTime.Now;
                    _context.FieldValues.Update(fieldValue);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error setting field value for instance: {InstanceId}, field: {FieldId}", instanceId, fieldDefinitionId);
                throw;
            }
        }

        public async Task<FieldValue?> GetFieldValueAsync(int instanceId, int fieldDefinitionId)
        {
            try
            {
                return await _context.FieldValues
                    .Include(v => v.FieldDefinition)
                    .FirstOrDefaultAsync(v => v.FormInstanceId == instanceId && v.FieldDefinitionId == fieldDefinitionId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting field value for instance: {InstanceId}, field: {FieldId}", instanceId, fieldDefinitionId);
                throw;
            }
        }

        public async Task<IEnumerable<FieldValue>> GetAllFieldValuesAsync(int instanceId)
        {
            try
            {
                return await _context.FieldValues
                    .Include(v => v.FieldDefinition)
                    .Where(v => v.FormInstanceId == instanceId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting all field values for instance: {InstanceId}", instanceId);
                throw;
            }
        }

        public async Task<bool> CompleteInstanceAsync(int instanceId)
        {
            try
            {
                var instance = await _context.FormInstances.FindAsync(instanceId);
                if (instance == null) return false;

                instance.Status = "Completed";
                instance.CompletedDate = DateTime.Now;
                instance.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                Log.Information("Form instance completed: {Id}", instanceId);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error completing form instance: {Id}", instanceId);
                throw;
            }
        }

        public async Task<bool> ApproveInstanceAsync(int instanceId, int approvedById)
        {
            try
            {
                var instance = await _context.FormInstances.FindAsync(instanceId);
                if (instance == null) return false;

                instance.Status = "Approved";
                instance.ApprovedById = approvedById;
                instance.ApprovedDate = DateTime.Now;
                instance.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                Log.Information("Form instance approved: {Id} by user: {UserId}", instanceId, approvedById);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error approving form instance: {Id}", instanceId);
                throw;
            }
        }

        public async Task<string> GenerateInstanceNumberAsync()
        {
            try
            {
                var date = DateTime.Now;
                var prefix = $"FORM-{date:yyyyMMdd}";

                var lastInstance = await _context.FormInstances
                    .Where(i => i.InstanceNumber != null && i.InstanceNumber.StartsWith(prefix))
                    .OrderByDescending(i => i.InstanceNumber)
                    .FirstOrDefaultAsync();

                int sequence = 1;
                if (lastInstance != null && !string.IsNullOrEmpty(lastInstance.InstanceNumber))
                {
                    var parts = lastInstance.InstanceNumber.Split('-');
                    if (parts.Length > 2 && int.TryParse(parts[2], out int lastSeq))
                    {
                        sequence = lastSeq + 1;
                    }
                }

                return $"{prefix}-{sequence:D4}";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error generating instance number");
                throw;
            }
        }
    }
}
