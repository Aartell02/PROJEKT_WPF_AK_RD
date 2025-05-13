using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using PROJEKT_WPF_AK_RD.Services;
using System.Xml.Linq;
using System.Diagnostics;


namespace PROJEKT_WPF_AK_RD.Views
{
    public partial class getquestionmockupview : Window
    {
        private readonly APIQuestionService _apiService = new();
        public getquestionmockupview()
        {
            InitializeComponent();
            LoadOptions();
        }
        private async void FetchQuestions_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(AmountBox.Text, out int amount);
            if (amount <= 0) amount = 5;

            string? difficulty = (DifficultyBox.SelectedItem as String) is "any" ? null : (DifficultyBox.SelectedItem as String);
            string? type = (TypeBox.SelectedItem as String) is "any" ? null : (TypeBox.SelectedItem as String);
            int? category = (CategoryBox.SelectedItem as TriviaCategory) is { Id: > 0 } cat ? cat.Id : null;

            try
            {
                var response = await _apiService.GetQuestionsAsync(amount, category, difficulty, type);

                QuestionsList.Items.Clear();
                if (response?.results != null)
                {
                    foreach (var question in response.results)
                    {
                        QuestionsList.Items.Add(System.Net.WebUtility.HtmlDecode(question.question));
                    }
                }
                else
                {
                    QuestionsList.Items.Add("Brak wyników.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania pytań: " + ex.Message);
            }
        }
        private async void LoadOptions()
        {
            var categories = await _apiService.GetCategoriesAsync();
            categories.Insert(0, new TriviaCategory { Id = 0, Name = "Any Category" });
            CategoryBox.ItemsSource = categories;
            CategoryBox.SelectedIndex = 0;

            DifficultyBox.ItemsSource = new List<string> { "any", "easy", "medium", "hard" };
            DifficultyBox.SelectedIndex = 0;

            TypeBox.ItemsSource = new List<string> { "any", "multiple", "boolean" };
            TypeBox.SelectedIndex = 0;
        }
    }
}
