using PROJEKT_WPF_AK_RD.Services;
using System.Windows;
using System.Windows.Controls;
using PROJEKT_WPF_AK_RD.ViewModels;

namespace PROJEKT_WPF_AK_RD.Views
{
    /// <summary>
    /// Interaction logic for GetQuestionsWindow.xaml
    /// </summary>
    public partial class GetQuestionsView : UserControl
    {
        private readonly APIQuestionService _apiService = new();
        private List<TriviaQuestion> _questions = new();

        public GetQuestionsView()
        {
            InitializeComponent();
            LoadOptions();
        }

        // Pobiera pytania z API i wyświetla je na liście
        private async void FetchQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AmountBox.Text, out int amount) || amount <= 0)
                amount = 5;

            string? difficulty = (DifficultyBox.SelectedItem as string) == "any" ? null : DifficultyBox.SelectedItem as string;
            int? category = (CategoryBox.SelectedItem as TriviaCategory) is { Id: > 0 } cat ? cat.Id : null;

            try
            {
                var response = await _apiService.GetQuestionsAsync(amount, category, difficulty);

                QuestionsListBox.Items.Clear();
                _questions.Clear();

                if (response?.results is { Count: > 0 })
                {
                    foreach (var question in response.results)
                    {
                        QuestionsListBox.Items.Add(System.Net.WebUtility.HtmlDecode(question.question));
                        _questions.Add(question);
                    }
                }
                else
                {
                    QuestionsListBox.Items.Add("Brak wyników.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania pytań: " + ex.Message);
            }
        }

        // Rozpoczyna quiz, jeśli pytania są dostępne
        private void PlayQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (_questions.Count > 0)
            {
                var gameWindow = new PlayQuizView(_questions);
            }
            else
            {
                MessageBox.Show("Najpierw pobierz pytania.");
            }
        }

        // Ładuje opcje do pól wyboru
        private async void LoadOptions()
        {
            var categories = await _apiService.GetCategoriesAsync();
            categories.Insert(0, new TriviaCategory { Id = 0, Name = "Any Category" });

            CategoryBox.ItemsSource = categories;
            CategoryBox.SelectedIndex = 0;

            DifficultyBox.ItemsSource = new List<string> { "any", "easy", "medium", "hard" };
            DifficultyBox.SelectedIndex = 0;
        }
    }
}
