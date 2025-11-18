using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastForm.Models;
using FastForm.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace FastForm.ViewModels
{
    public partial class FormEditorViewModel : ViewModelBase
    {
        private readonly IFormTemplateService _templateService;
        private readonly IFieldConfigurationService _configService;

        [ObservableProperty]
        private FormTemplate? _currentTemplate;

        [ObservableProperty]
        private FieldDefinition? _selectedField;

        [ObservableProperty]
        private ObservableCollection<FieldDefinition> _fields = new();

        [ObservableProperty]
        private string? _templateImagePath;

        [ObservableProperty]
        private bool _isEditMode;

        public FormEditorViewModel(IFormTemplateService templateService, IFieldConfigurationService configService)
        {
            _templateService = templateService;
            _configService = configService;
        }

        [RelayCommand]
        private async Task LoadTemplateAsync(int templateId)
        {
            try
            {
                CurrentTemplate = await _templateService.GetTemplateByIdAsync(templateId);
                if (CurrentTemplate != null)
                {
                    var fields = await _templateService.GetFieldDefinitionsAsync(templateId);
                    Fields = new ObservableCollection<FieldDefinition>(fields);
                    TemplateImagePath = CurrentTemplate.ImagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Şablon yüklenirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void SelectImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
                Title = "Form Resmi Seç"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TemplateImagePath = openFileDialog.FileName;
            }
        }

        [RelayCommand]
        private void AddField()
        {
            var newField = new FieldDefinition
            {
                FormTemplateId = CurrentTemplate?.Id ?? 0,
                FieldName = $"Field{Fields.Count + 1}",
                FieldLabel = "Yeni Alan",
                FieldType = "Text",
                X = 50,
                Y = 50,
                Width = 200,
                Height = 30,
                FontFamily = "Arial",
                FontSize = 12,
                FontColor = "#000000",
                TextAlignment = "Left"
            };

            Fields.Add(newField);
            SelectedField = newField;
            IsEditMode = true;
        }

        [RelayCommand]
        private async Task SaveFieldAsync()
        {
            if (SelectedField == null || CurrentTemplate == null) return;

            try
            {
                if (SelectedField.Id == 0)
                {
                    SelectedField.FormTemplateId = CurrentTemplate.Id;
                    await _templateService.AddFieldDefinitionAsync(SelectedField);
                }
                else
                {
                    await _templateService.UpdateFieldDefinitionAsync(SelectedField);
                }

                MessageBox.Show("Alan kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                IsEditMode = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Alan kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteFieldAsync()
        {
            if (SelectedField == null) return;

            var result = MessageBox.Show("Bu alanı silmek istediğinizden emin misiniz?", "Onay",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (SelectedField.Id > 0)
                    {
                        await _templateService.DeleteFieldDefinitionAsync(SelectedField.Id);
                    }

                    Fields.Remove(SelectedField);
                    SelectedField = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alan silinirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private async Task SaveTemplateAsync()
        {
            if (CurrentTemplate == null) return;

            try
            {
                // Save template
                if (!string.IsNullOrEmpty(TemplateImagePath) && File.Exists(TemplateImagePath))
                {
                    CurrentTemplate.ImageData = await File.ReadAllBytesAsync(TemplateImagePath);
                }

                if (CurrentTemplate.Id == 0)
                {
                    await _templateService.CreateTemplateAsync(CurrentTemplate);
                }
                else
                {
                    await _templateService.UpdateTemplateAsync(CurrentTemplate);
                }

                // Save configuration
                var configJson = await _configService.ExportConfigurationAsync(CurrentTemplate.Id);
                await _templateService.SaveConfigurationAsync(CurrentTemplate.Id, configJson, "Şablon kaydedildi");

                MessageBox.Show("Şablon kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Şablon kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task ExportConfigurationAsync()
        {
            if (CurrentTemplate == null) return;

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    Title = "Konfigürasyonu Dışa Aktar",
                    FileName = $"{CurrentTemplate.Name}_config.json"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var configJson = await _configService.ExportConfigurationAsync(CurrentTemplate.Id);
                    await File.WriteAllTextAsync(saveFileDialog.FileName, configJson);
                    MessageBox.Show("Konfigürasyon dışa aktarıldı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Konfigürasyon dışa aktarılırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task ImportConfigurationAsync()
        {
            if (CurrentTemplate == null) return;

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    Title = "Konfigürasyon İçe Aktar"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var configJson = await File.ReadAllTextAsync(openFileDialog.FileName);
                    await _configService.ImportConfigurationAsync(CurrentTemplate.Id, configJson);

                    // Reload fields
                    await LoadTemplateAsync(CurrentTemplate.Id);

                    MessageBox.Show("Konfigürasyon içe aktarıldı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Konfigürasyon içe aktarılırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
