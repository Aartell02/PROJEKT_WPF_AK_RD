using PROJEKT_WPF_AK_RD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROJEKT_WPF_AK_RD.Views
{
    /// <summary>
    /// Interaction logic for PlayQuizWindow.xaml
    /// </summary>
    public partial class PlayQuizView : UserControl
    {
        private List<TriviaQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        public PlayQuizView(List<TriviaQuestion> questions)
        {
            InitializeComponent();
            _questions = questions;
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                var result = MessageBox.Show(
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
                return;
            }

            var q = _questions[_currentIndex];

            // Decode question text
            string questionText = WebUtility.HtmlDecode(q.question);
            QuestionTextBlock.Text = $"Question {_currentIndex + 1} of {_questions.Count}:\n\n{questionText}";

            // Decode and shuffle answers
            var allAnswers = new List<string>(q.incorrect_answers.Select(a => WebUtility.HtmlDecode(a)));
            allAnswers.Add(WebUtility.HtmlDecode(q.correct_answer));
            var shuffledAnswers = allAnswers.OrderBy(a => Guid.NewGuid()).ToList();

            // Display answers
            AnswerButton1.Content = shuffledAnswers[0];
            AnswerButton2.Content = shuffledAnswers[1];
            AnswerButton3.Content = shuffledAnswers[2];
            AnswerButton4.Content = shuffledAnswers[3];

            ScoreTextBlock.Text = $"Score: {_score}/{_questions.Count}";
        }



        private async void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            var chosenAnswer = clickedButton.Content.ToString();
            var correctAnswer = _questions[_currentIndex].correct_answer;

            var allButtons = new[] { AnswerButton1, AnswerButton2, AnswerButton3, AnswerButton4 };

            // Ocen odpowiedź i pokoloruj
            if (chosenAnswer == correctAnswer)
            {
                clickedButton.Background = Brushes.LightGreen;
                _score++;
            }
            else
            {
                clickedButton.Background = Brushes.IndianRed;

                foreach (var btn in allButtons)
                {
                    if (btn.Content.ToString() == correctAnswer)
                    {
                        btn.Background = Brushes.LightGreen;
                        break;
                    }
                }
            }

            await Task.Delay(2000);

            _currentIndex++;
            DisplayQuestion();

            // Po załadowaniu nowego pytania – reset kolorów
            foreach (var btn in allButtons)
            {
                btn.Background = Brushes.White;
            }
        }
    }
}