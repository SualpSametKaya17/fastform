using FastForm.Models;

namespace FastForm.Services
{
    public interface IFormDataService
    {
        Task<IEnumerable<FormInstance>> GetAllInstancesAsync(int? templateId = null, string? status = null);
        Task<FormInstance?> GetInstanceByIdAsync(int id);
        Task<FormInstance> CreateInstanceAsync(FormInstance instance);
        Task<FormInstance> UpdateInstanceAsync(FormInstance instance);
        Task<bool> DeleteInstanceAsync(int id);
        Task<bool> SetFieldValueAsync(int instanceId, int fieldDefinitionId, string? value, byte[]? binaryData = null);
        Task<FieldValue?> GetFieldValueAsync(int instanceId, int fieldDefinitionId);
        Task<IEnumerable<FieldValue>> GetAllFieldValuesAsync(int instanceId);
        Task<bool> CompleteInstanceAsync(int instanceId);
        Task<bool> ApproveInstanceAsync(int instanceId, int approvedById);
        Task<string> GenerateInstanceNumberAsync();
    }
}
