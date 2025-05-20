using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.EntityFramework;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    class RegisterViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;
        private string _username;
        private string _password;
        private string _repeatPassword;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(arePasswordsEqual));
            }
        }
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                _repeatPassword = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(arePasswordsEqual));
            }
        }
        public bool arePasswordsEqual => _password == _repeatPassword ? true : false;
        public ICommand RegisterCommand { get; }
        public RegisterViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            RegisterCommand = new RelayCommand(Register);
        }
        private void Register()
        {
            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == Username);
            if (user == null && arePasswordsEqual)
            {
                user = new User
                {
                    Username = _username,
                    Password = _password
                };

                db.Add(user);
                db.SaveChanges();
                _mainViewModel.CurrentView = new LoginView(_mainViewModel);
            }
            else
            {
                MessageBox.Show("User already exist");
            }
        }
    }
}
