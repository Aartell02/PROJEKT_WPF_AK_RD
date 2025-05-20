using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.Views;
using PROJEKT_WPF_AK_RD.Models;
using PROJEKT_WPF_AK_RD.EntityFramework;
using System.Windows.Input;
using System.Windows;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.Views;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    class PasswordResetViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;
        private string _username;
        private string _password;
        private string _repeatPassword;

        public string Username
        {
            get => _username;
            set { _username = value; 
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set { _password = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(arePasswordsEqual));
            }
        }
        public string RepeatPassword
        {
            get => _repeatPassword;
            set { _repeatPassword = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(arePasswordsEqual));
            }
        }
        public bool arePasswordsEqual => _password == _repeatPassword ? true : false;
        public ICommand ResetCommand { get; }
        public PasswordResetViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ResetCommand = new RelayCommand(Reset);
        }
        private void Reset()
        {
            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == Username);
            if (user != null && arePasswordsEqual)
            {
                user.Password = _password;
                db.Users.Update(user);
                db.SaveChanges();
                _mainViewModel.CurrentView = new LoginView(_mainViewModel);
            }
            else
            {
                MessageBox.Show("Username incorrect");
            }
        }
    }
}
