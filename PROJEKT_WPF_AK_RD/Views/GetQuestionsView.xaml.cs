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
    public partial class GetQuestionsView : System.Windows.Controls.UserControl
    {
        public GetQuestionsView(MainViewModel _mainViewModel)
        {
            this.DataContext = new GetQusetionsViewModel(_mainViewModel);
            InitializeComponent();
            LoadOptions();
        }

        private async void FetchQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AmountBox.Text, out int amount) || amount <= 0) amount = 5;

            string? difficulty = (DifficultyBox.SelectedItem as string) == "any" ? null : DifficultyBox.SelectedItem as string;
            int? category = (CategoryBox.SelectedItem as TriviaCategory) is { Id: > 0 } cat ? cat.Id : null;
            QuestionsListBox.Items.Clear();
            if (this.DataContext is GetQusetionsViewModel viewModel)
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

        private async void LoadOptions()
        {
            if (this.DataContext is GetQusetionsViewModel viewModel)
            {
                try
                {
                    var categories = await viewModel._apiService.GetCategoriesAsync();
                    categories.Insert(0, new TriviaCategory { Id = 0, Name = "Any Category" });

                    CategoryBox.ItemsSource = categories;
                    CategoryBox.SelectedIndex = 0;

                    DifficultyBox.ItemsSource = new List<string> { "any", "easy", "medium", "hard" };
                    DifficultyBox.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("API SERVICE ERROR" + ex.Message);
                }
            }
        }
    }
}
