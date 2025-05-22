using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.Models;
using PROJEKT_WPF_AK_RD.Views;
using PROJEKT_WPF_AK_RD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class GetQusetionsViewModel : ViewModelBase
    {
        private List<TriviaQuestion> _questions = new();
        private MainViewModel _mainViewModel;
        public readonly APIQuestionService _apiService = new();
        public ICommand PlayCommand { get; }
        public ICommand PrintCommand { get; }

        public GetQusetionsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            PlayCommand = new RelayCommand(ShowPlayQuizCommand);
            PrintCommand = new RelayCommand(PrintQuestions);
        }
        private void ShowPlayQuizCommand()
        {
            if (_questions.Count > 0)
            {
                _mainViewModel.CurrentView = new PlayQuizView(_mainViewModel, _questions);
            }
            else
            {
                MessageBox.Show("Fetch questions first");
            }
        }
        public async Task<bool> FetchQuestions(int amount, int? category, string? difficulty)
        {
            try
            {
                var response = await _apiService.GetQuestionsAsync(amount, category, difficulty);
                _questions.Clear();
                if (response?.results is { Count: > 0 })
                {
                    foreach (var question in response.results)
                    {
                        _questions.Add(question);
                    }
                    return true;
                    
                }
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fetching error: " + ex.Message);
                return false;
            }
        }

        public List<TriviaQuestion> GetQuestions() => _questions;

        private void PrintQuestions()
        {
            if (_questions == null || _questions.Count == 0)
            {
                MessageBox.Show("No questions to print.");
                return;
            }

            var doc = new System.Windows.Documents.FlowDocument
            {
                PagePadding = new Thickness(50),
                ColumnWidth = double.PositiveInfinity // Ensures one column
            };

            int i = 1;

            foreach (var question in _questions)
            {
                doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                    new System.Windows.Documents.Bold(
                        new System.Windows.Documents.Run($"Pytanie {i++}: {System.Net.WebUtility.HtmlDecode(question.question)}")
                    )
                ));

                var allAnswers = new List<string>(question.incorrect_answers);
                allAnswers.Add(question.correct_answer);
                allAnswers = allAnswers.OrderBy(a => Guid.NewGuid()).ToList(); // random order

                foreach (var ans in allAnswers)
                {
                    doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                        new System.Windows.Documents.Run($"- {System.Net.WebUtility.HtmlDecode(ans)}")
                    ));
                }

                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run("")));
            }

            var pd = new System.Windows.Controls.PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintDocument(((System.Windows.Documents.IDocumentPaginatorSource)doc).DocumentPaginator, "Quiz");
            }
        }
    }
}
