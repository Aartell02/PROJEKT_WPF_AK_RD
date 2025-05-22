using System.Linq;
using System.Windows;
using PROJEKT_WPF_AK_RD.Models;
using Microsoft.EntityFrameworkCore;
using PROJEKT_WPF_AK_RD.Data;

namespace PROJEKT_WPF_AK_RD.Views
{
    public partial class AdminWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private List<UserWithScoreDto> _allUsers; // store full list

        public AdminWindow()
        {
            InitializeComponent();

            _dbContext = new AppDbContext();

            LoadUsers();
        }

        private void LoadUsers()
        {
            _allUsers = _dbContext.Users
                .Include(u => u.QuizGames)
                .AsNoTracking()
                .Select(u => new UserWithScoreDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    AverageScorePercentage = u.QuizGames.Any(q => q.MaxScore > 0)
                        ? u.QuizGames
                            .Where(q => q.MaxScore > 0)
                            .Average(q => (double)q.Score / q.MaxScore) * 100
                        : 0
                })
                .ToList();

            UserDataGrid.ItemsSource = _allUsers;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim().ToLower();

            var filtered = _allUsers
                .Where(u => u.Username != null && u.Username.ToLower().Contains(searchTerm))
                .ToList();

            UserDataGrid.ItemsSource = filtered;
        }
    }


    public class UserWithScoreDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public double AverageScorePercentage { get; set; } // in percentage
    }
}
