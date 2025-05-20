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
        public GetQusetionsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            PlayCommand = new RelayCommand(ShowPlayQuizCommand);
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

    }
}
