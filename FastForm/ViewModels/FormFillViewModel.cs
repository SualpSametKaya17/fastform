using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastForm.Models;
using FastForm.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace FastForm.ViewModels
{
    public partial class FormFillViewModel : ViewModelBase
    {
        private readonly IFormDataService _formDataService;
        private readonly IFormTemplateService _templateService;
        private readonly IExportService _exportService;

        [ObservableProperty]
        private FormInstance? _currentInstance;

        [ObservableProperty]
        private FormTemplate? _currentTemplate;

        [ObservableProperty]
        private ObservableCollection<FieldDefinition> _fields = new();

        [ObservableProperty]
        private ObservableCollection<FieldValue> _fieldValues = new();

        [ObservableProperty]
        private string? _templateImagePath;

        public FormFillViewModel(IFormDataService formDataService, IFormTemplateService templateService, IExportService exportService)
        {
            _formDataService = formDataService;
            _templateService = templateService;
            _exportService = exportService;
        }

        [RelayCommand]
        private async Task LoadInstanceAsync(int instanceId)
        {
            try
            {
                CurrentInstance = await _formDataService.GetInstanceByIdAsync(instanceId);
                if (CurrentInstance != null)
                {
                    CurrentTemplate = CurrentInstance.FormTemplate;
                    Fields = new ObservableCollection<FieldDefinition>(CurrentTemplate.FieldDefinitions);
                    FieldValues = new ObservableCollection<FieldValue>(CurrentInstance.FieldValues);
                    TemplateImagePath = CurrentTemplate.ImagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form yüklenirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task CreateNewInstanceAsync(int templateId)
        {
            try
            {
                CurrentTemplate = await _templateService.GetTemplateByIdAsync(templateId);
                if (CurrentTemplate == null) return;

                var instance = new FormInstance
                {
                    FormTemplateId = templateId,
                    InstanceName = $"Yeni Form - {DateTime.Now:dd.MM.yyyy HH:mm}",
                    Status = "Draft",
                    CreatedDate = DateTime.Now
                };

                CurrentInstance = await _formDataService.CreateInstanceAsync(instance);
                Fields = new ObservableCollection<FieldDefinition>(CurrentTemplate.FieldDefinitions);
                FieldValues = new ObservableCollection<FieldValue>();
                TemplateImagePath = CurrentTemplate.ImagePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yeni form oluşturulurken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveFieldValueAsync(FieldDefinition field, string value)
        {
            if (CurrentInstance == null) return;

            try
            {
                await _formDataService.SetFieldValueAsync(CurrentInstance.Id, field.Id, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Alan değeri kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveInstanceAsync()
        {
            if (CurrentInstance == null) return;

            try
            {
                await _formDataService.UpdateInstanceAsync(CurrentInstance);
                MessageBox.Show("Form kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task CompleteInstanceAsync()
        {
            if (CurrentInstance == null) return;

            var result = MessageBox.Show("Formu tamamlamak istediğinizden emin misiniz?", "Onay",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _formDataService.CompleteInstanceAsync(CurrentInstance.Id);
                    CurrentInstance.Status = "Completed";
                    CurrentInstance.CompletedDate = DateTime.Now;

                    MessageBox.Show("Form tamamlandı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Form tamamlanırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private async Task ExportInstanceAsync(ExportFormat format)
        {
            if (CurrentInstance == null) return;

            try
            {
                var extension = format.ToString().ToLower();
                var fileName = $"{CurrentInstance.InstanceNumber}_{DateTime.Now:yyyyMMddHHmmss}.{extension}";
                var outputPath = Path.Combine("output", fileName);

                await _exportService.ExportFormInstanceAsync(CurrentInstance.Id, format, outputPath);

                MessageBox.Show($"Form dışa aktarıldı: {outputPath}", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form dışa aktarılırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task PreviewInstanceAsync()
        {
            if (CurrentInstance == null) return;

            try
            {
                // Generate preview image
                var previewBytes = await _exportService.ExportFormInstanceToByteArrayAsync(CurrentInstance.Id, ExportFormat.PNG);

                // Show preview window (would be implemented separately)
                MessageBox.Show("Önizleme penceresi açılacak (henüz uygulanmadı)", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Önizleme oluşturulurken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
