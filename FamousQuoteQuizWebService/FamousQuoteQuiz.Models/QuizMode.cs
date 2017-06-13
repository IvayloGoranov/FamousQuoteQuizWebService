using System.ComponentModel.DataAnnotations;

namespace FamousQuoteQuiz.Models
{
    public class QuizMode : BaseModel<int>
    {
        [Required]
        public QuizModeType Type { get; set; }

        public bool IsSelected { get; set; }
    }
}
