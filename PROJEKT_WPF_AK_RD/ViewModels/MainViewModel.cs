using PROJEKT_WPF_AK_RD.EntityFramework;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.Views;

using System.Windows.Input;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        private User _user;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowQuestionsViewCommand { get; }
        public ICommand ShowHistoryViewCommand { get; }
        public ICommand ShowGetQusetionsViewCommand { get; }

        public MainViewModel()
        {
            ShowLoginViewCommand = new RelayCommand(ShowLoginView);
            ShowHistoryViewCommand = new RelayCommand(ShowHistoryView);
            ShowGetQusetionsViewCommand = new RelayCommand(ShowGetQuestionsView);
            ShowLoginView();
        }

        private void ShowLoginView()
        {
            CurrentView = new LoginView(this);
        }
        private void ShowGetQuestionsView()
        {
            if (_user == null) CurrentView = new LoginView(this);
            else CurrentView = new GetQuestionsView(this);
        }
        private void ShowHistoryView()
        {
            if(_user == null) CurrentView = new LoginView(this);
            else CurrentView = new QuizHistoryView(this);
        }

        public void LoginUser(User user)
        {
            _user = user;
        }
        public void LogoutUser()
        {
            _user = null;
        }
    }
}
