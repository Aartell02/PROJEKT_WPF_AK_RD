using PROJEKT_WPF_AK_RD.Services;
using PROJEKT_WPF_AK_RD.Models;
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


        public GetQuestionsView(MainViewModel _mainViewModel)
        {
            this.DataContext = new QuizViewModel(_mainViewModel);
            InitializeComponent();
            LoadOptions();
        }

        // Pobiera pytania z API i wyświetla je na liście
        private async void FetchQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AmountBox.Text, out int amount) || amount <= 0) amount = 5;

            string? difficulty = (DifficultyBox.SelectedItem as string) == "any" ? null : DifficultyBox.SelectedItem as string;
            int? category = (CategoryBox.SelectedItem as TriviaCategory) is { Id: > 0 } cat ? cat.Id : null;
            QuestionsListBox.Items.Clear();
            if (this.DataContext is QuizViewModel viewModel)
            {
                if(await viewModel.FetchQuestions(amount, category, difficulty))
                {
                    foreach(TriviaQuestion question in viewModel.GetQuestions())
                    QuestionsListBox.Items.Add(System.Net.WebUtility.HtmlDecode(question.question));
                }
                else
                {
                    QuestionsListBox.Items.Add("Brak wyników.");
                }
            }
        }

        // Rozpoczyna quiz, jeśli pytania są dostępne


        // Ładuje opcje do pól wyboru
        private async void LoadOptions()
        {
            if (this.DataContext is QuizViewModel viewModel)
            {
                var categories = await viewModel._apiService.GetCategoriesAsync();
                categories.Insert(0, new TriviaCategory { Id = 0, Name = "Any Category" });

                CategoryBox.ItemsSource = categories;
                CategoryBox.SelectedIndex = 0;

                DifficultyBox.ItemsSource = new List<string> { "any", "easy", "medium", "hard" };
                DifficultyBox.SelectedIndex = 0;
            }
        }
    }
}
