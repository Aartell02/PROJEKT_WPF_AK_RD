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

        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowQuestionsViewCommand { get; }
        public MainViewModel()
        {
            ShowLoginViewCommand = new RelayCommand(ShowLoginView);
            ShowQuestionsViewCommand = new RelayCommand(ShowQuestionsView);
            ShowLoginView();
        }

        private void ShowLoginView()
        {
            CurrentView = new LoginView(this);
        }
        private void ShowQuestionsView()
        {
            CurrentView = new GetQuestionsView();
        }

    }
}
