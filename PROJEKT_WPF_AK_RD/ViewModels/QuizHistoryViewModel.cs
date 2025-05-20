using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.EntityFramework;
using PROJEKT_WPF_AK_RD.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class QuizHistoryViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;
        public ObservableCollection<QuizGame> Games { get; set; } = new ObservableCollection<QuizGame>();

        public ICommand ClearHistoryCommand { get; }

        public QuizHistoryViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ClearHistoryCommand = new RelayCommand(ClearHistory);
            LoadGames();
        }

        private void LoadGames()
        {
            using var db = new AppDbContext();

            var userId = _mainViewModel.User?.Id;

            if (userId == -1)
            {
                MessageBox.Show("No account detected");
                return;
            }

            var games = db.QuizGames
                          .Where(g => g.UserId == userId)
                          .OrderByDescending(g => g.Date)
                          .ToList();

            Games.Clear();
            foreach (var game in games)
                Games.Add(game);
        }
        private void ClearHistory()
        {
            using var db = new AppDbContext();

            var userGames = db.QuizGames.Where(g => g.UserId == _mainViewModel.User.Id);
            db.QuizGames.RemoveRange(userGames);
            db.SaveChanges();

            Games.Clear();
        }

    }
}
