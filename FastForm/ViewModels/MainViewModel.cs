using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastForm.Services;
using System.Windows;

namespace FastForm.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IFormTemplateService _templateService;
        private readonly IFormDataService _formDataService;

        [ObservableProperty]
        private ViewModelBase? _currentViewModel;

        [ObservableProperty]
        private string _currentPageTitle = "Dashboard";

        [ObservableProperty]
        private int _totalTemplates;

        [ObservableProperty]
        private int _totalForms;

        [ObservableProperty]
        private int _pendingForms;

        public MainViewModel(IFormTemplateService templateService, IFormDataService formDataService)
        {
            _templateService = templateService;
            _formDataService = formDataService;

            LoadStatisticsAsync();
        }

        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentPageTitle = "Kontrol Paneli";
            // Load dashboard view model
            LoadStatisticsAsync();
        }

        [RelayCommand]
        private void NavigateToTemplates()
        {
            CurrentPageTitle = "Form Şablonları";
            // Load templates view model
        }

        [RelayCommand]
        private void NavigateToForms()
        {
            CurrentPageTitle = "Formlar";
            // Load forms view model
        }

        [RelayCommand]
        private void NavigateToSettings()
        {
            CurrentPageTitle = "Ayarlar";
            // Load settings view model
        }

        [RelayCommand]
        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private async void LoadStatisticsAsync()
        {
            try
            {
                var templates = await _templateService.GetAllTemplatesAsync();
                TotalTemplates = templates.Count();

                var forms = await _formDataService.GetAllInstancesAsync();
                TotalForms = forms.Count();
                PendingForms = forms.Count(f => f.Status == "Draft" || f.Status == "InProgress");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İstatistikler yüklenirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
