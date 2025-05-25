using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PROJEKT_WPF_AK_RD.ViewModels;

namespace PROJEKT_WPF_AK_RD.Views
{
    /// <summary>
    /// Logika interakcji dla klasy LoginView.xaml
    /// </summary>
    public partial class LoginView : System.Windows.Controls.UserControl
    {
        public LoginView( MainViewModel _mainViewModel)
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(_mainViewModel);
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
