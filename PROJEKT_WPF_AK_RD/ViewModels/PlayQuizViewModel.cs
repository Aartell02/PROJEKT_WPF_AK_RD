using PROJEKT_WPF_AK_RD.Data;
using PROJEKT_WPF_AK_RD.Helpers;
using PROJEKT_WPF_AK_RD.Models;
using PROJEKT_WPF_AK_RD.Views;
using PROJEKT_WPF_AK_RD.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PROJEKT_WPF_AK_RD.ViewModels
{
    public class PlayQuizViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;
        private List<TriviaQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private string _questionText;
        private string _scoreText;
        private string _correctAnswer;

        public string QuestionText
        {
            get => _questionText;
            set { _questionText = value; OnPropertyChanged(); }
        }

        public string ScoreText
        {
            get => _scoreText;
            set { _scoreText = value; OnPropertyChanged(); }
        }

        public ObservableCollection<AnswerOption> AnswerOptions { get; set; } = new();

        public ICommand AnswerCommand { get; }

        public PlayQuizViewModel(MainViewModel mainViewModel, List<TriviaQuestion> questions)
        {
            _mainViewModel = mainViewModel;
            _questions = questions;
            AnswerCommand = new AsyncRelayCommand(async param => await EvaluateAnswer((string)param));
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                using (var db = new AppDbContext())
                {
                    var quizGame = new QuizGame
                    {
                        Category = _questions.FirstOrDefault()?.category ?? "Unknown",
                        Score = _score,
                        MaxScore = _questions.Count,
                        Date = DateTime.Now,
                        UserId = _mainViewModel.User.Id
                    };

                    db.QuizGames.Add(quizGame);
                    db.SaveChanges();
                }

                var result = System.Windows.MessageBox.Show(
                    $"Quiz finished! Your score: {_score}/{_questions.Count}\n\nDo you want to try the same quiz again?",
                    "Quiz Completed",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    _currentIndex = 0;
                    _score = 0;
                    DisplayQuestion();
                }
                else
                {
                    _mainViewModel.CurrentView = new GetQuestionsView(_mainViewModel);
                    return;
                }
            }

            var q = _questions[_currentIndex];
            _correctAnswer = WebUtility.HtmlDecode(q.correct_answer);

            QuestionText = $"Question {_currentIndex + 1} of {_questions.Count}:\n\n{WebUtility.HtmlDecode(q.question)}";
            ScoreText = $"Score: {_score}/{_questions.Count}";

            var answers = q.incorrect_answers.Select(a => WebUtility.HtmlDecode(a)).ToList();
            answers.Add(_correctAnswer);
            var shuffled = answers.OrderBy(a => Guid.NewGuid()).ToList();

            AnswerOptions.Clear();
            foreach (var ans in shuffled)
            {
                AnswerOptions.Add(new AnswerOption { Text = ans, IsEnabled = true });
            }

        }

        private async Task EvaluateAnswer(string selected)
        {
            foreach (var opt in AnswerOptions)
            {
                opt.IsEnabled = false;
            }

            if (selected == _correctAnswer)
            {
                _score++;
            }

            ScoreText = $"Score: {_score}/{_questions.Count}";
            await Task.Delay(2000);

            _currentIndex++;
            DisplayQuestion();
        }
    }

    public class AnswerOption : ViewModelBase
    {
        public string Text { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set { _isEnabled = value; OnPropertyChanged(); }
        }
    }

}
