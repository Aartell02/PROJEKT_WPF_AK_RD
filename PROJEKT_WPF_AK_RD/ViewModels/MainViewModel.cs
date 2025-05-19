using PROJEKT_WPF_AK_RD.EntityFramework;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.Views;

using System.Windows.Input;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private User? _user;
        public User User
        {
            get => _user;
            set
            {
                if (_user == null)
                {
                    _user = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LoginButtonText));
                }
                else
                {
                    _user = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LoginButtonText));
                }
            }
        }
        public string LoginButtonText => _user != null ? "Logout" : "Login";

        public ICommand ShowLoginViewCommand { get; }
        public ICommand ToggleLoginCommand { get; }
        public MainViewModel()
        {
            ShowLoginViewCommand = new RelayCommand(ShowLoginView);
            ToggleLoginCommand = new RelayCommand(ToggleLogin);
            ShowLoginView();
        }

        private void ShowLoginView()
        {
            CurrentView = new LoginView(this);
        }
        public void ToggleLogin()
        {
            if (User != null) {
                User = null;
                ShowLoginView();
            }
            else
            {
                ShowLoginView();
            }
        }
    }
}
