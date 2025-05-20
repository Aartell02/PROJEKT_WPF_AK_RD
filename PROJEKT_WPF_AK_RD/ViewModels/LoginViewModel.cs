using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.Views;
using PROJEKT_WPF_AK_RD.Models;
using System.Windows.Input;
using System.Windows;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.EntityFramework;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;
        private string _username;
        private string _password;
        private string _statusMessage;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowPasswordResetCommand { get; }

        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoginCommand = new RelayCommand(Login);
            ShowRegisterCommand = new RelayCommand(ShowRegister);
            ShowPasswordResetCommand = new RelayCommand(ShowPasswordReset);
        }

        private void Login()
        {
            if (_mainViewModel.User == null )
            {
                using var db = new AppDbContext();
                var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
                if ( user != null)
                {
                    StatusMessage = $"Zalogowano jako {user.Username}";
                    MessageBox.Show("Logowanie udane!");
                    _mainViewModel.CurrentView = new GetQuestionsView(_mainViewModel);
                    _mainViewModel.User = user;
                }
                else
                {
                    StatusMessage = "Błędna nazwa użytkownika lub hasło";
                }
            }
            else
            {
                _mainViewModel.User = null;
            }
        }
        private void ShowRegister()
        {
            _mainViewModel.CurrentView = new RegisterView(_mainViewModel);
        }
        private void ShowPasswordReset()
        {
            _mainViewModel.CurrentView = new PasswordResetView(_mainViewModel);
        }
    }

    
}
