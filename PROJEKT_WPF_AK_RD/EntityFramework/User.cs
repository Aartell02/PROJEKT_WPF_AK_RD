
using System.ComponentModel.DataAnnotations;


namespace PROJEKT_WPF_AK_RD.EntityFramework
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        public string? Password{ get; set; }

        public virtual ICollection<QuizGame> QuizGames { get; set; } = new List<QuizGame>();
    }
}
