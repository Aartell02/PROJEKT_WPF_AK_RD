using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJEKT_WPF_AK_RD.EntityFramework
{
    public class QuizGame
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
