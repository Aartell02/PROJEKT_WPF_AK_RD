using PROJEKT_WPF_AK_RD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROJEKT_WPF_AK_RD.Views
{
    /// <summary>
    /// Interaction logic for QuizHistory.xaml
    /// </summary>
    public partial class QuizHistoryView : System.Windows.Controls.UserControl
    {
        public QuizHistoryView(MainViewModel _mainViewModel)
        {
            this.DataContext = new QuizHistoryViewModel(_mainViewModel);
            InitializeComponent();
        }
    }
}
