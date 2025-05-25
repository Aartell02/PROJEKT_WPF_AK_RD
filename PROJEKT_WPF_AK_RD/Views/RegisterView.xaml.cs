using System.Windows;
using System.Windows.Controls;
using PROJEKT_WPF_AK_RD.ViewModels;


namespace PROJEKT_WPF_AK_RD.Views
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterView.xaml
    /// </summary>
    public partial class RegisterView : System.Windows.Controls.UserControl
    {
        public RegisterView(MainViewModel _mainViewModel)
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel(_mainViewModel);
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is RegisterViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
        private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is RegisterViewModel viewModel)
            {
                viewModel.RepeatPassword = ((PasswordBox)sender).Password;
            }
        }

    }
}
