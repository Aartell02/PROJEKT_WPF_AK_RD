﻿using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.Views;
using PROJEKT_WPF_AK_RD.Models;
using System.Windows.Input;
using System.Windows;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.EntityFramework;
using System.Net.NetworkInformation;

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
            using var db = new AppDbContext();
            if (!db.Database.CanConnect())
            {
                System.Windows.MessageBox.Show("Database error occured. Application will shutdown soon.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                System.Windows.Application.Current.Shutdown(); 
            }
            /*try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 3000);
                }
            }
            catch
            {
                MessageBox.Show("Internet connection error occured. Application will shutdown soon.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                Application.Current.Shutdown();
            }*/

        }

        private void Login()
        {
            if (_mainViewModel.User == null )
            {
                using var db = new AppDbContext();
                var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
                if (Username == "admin" && Password == "admin")
                {
                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                }
                else if (user != null)
                {
                    StatusMessage = $"Logged in as {user.Username}";
                    System.Windows.MessageBox.Show("Logged in succesfuly!");
                    _mainViewModel.CurrentView = new GetQuestionsView(_mainViewModel);
                    _mainViewModel.User = user;
                }
                else
                {
                    StatusMessage = "User does not exist!";
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
