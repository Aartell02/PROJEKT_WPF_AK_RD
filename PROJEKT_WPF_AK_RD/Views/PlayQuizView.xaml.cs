using PROJEKT_WPF_AK_RD.Models;
using PROJEKT_WPF_AK_RD.ViewModels;
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
    public partial class PlayQuizView : System.Windows.Controls.UserControl
    {
        public PlayQuizView(MainViewModel mainViewModel, List<TriviaQuestion> questions)
        {
            InitializeComponent();
            DataContext = new PlayQuizViewModel(mainViewModel, questions);
        }
    }
}