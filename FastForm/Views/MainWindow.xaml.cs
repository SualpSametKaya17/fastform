using FastForm.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FastForm.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}
