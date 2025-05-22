using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.EntityFramework;
using PROJEKT_WPF_AK_RD.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class QuizHistoryViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private string _selectedCategory;

        public ObservableCollection<QuizGame> Games { get; set; } = new ObservableCollection<QuizGame>();
        private List<QuizGame> _allGames = new List<QuizGame>();

        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();

        public ICommand ClearHistoryCommand { get; }

        public QuizHistoryViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ClearHistoryCommand = new RelayCommand(ClearHistory);
            LoadGames();
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                FilterGames();
            }
        }

        private void LoadGames()
        {
            using var db = new AppDbContext();
            var userId = _mainViewModel.User?.Id ?? -1;

            if (userId == -1)
            {
                MessageBox.Show("No account detected");
                return;
            }

            _allGames = db.QuizGames
                          .Where(g => g.UserId == userId)
                          .OrderByDescending(g => g.Date)
                          .ToList();

            // Populate categories
            var categories = _allGames.Select(g => g.Category).Distinct().OrderBy(c => c).ToList();
            Categories.Clear();
            Categories.Add("All");  // Optional "All" filter
            foreach (var category in categories)
                Categories.Add(category);

            SelectedCategory = "All";  // default value
        }

        private void FilterGames()
        {
            Games.Clear();

            IEnumerable<QuizGame> filtered = _allGames;

            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
            {
                filtered = _allGames.Where(g => g.Category == SelectedCategory);
            }

            foreach (var game in filtered)
                Games.Add(game);
        }

        private void ClearHistory()
        {
            using var db = new AppDbContext();

            var userGames = db.QuizGames.Where(g => g.UserId == _mainViewModel.User.Id);
            db.QuizGames.RemoveRange(userGames);
            db.SaveChanges();

            Games.Clear();
            _allGames.Clear();
            Categories.Clear();
        }
    }
}
