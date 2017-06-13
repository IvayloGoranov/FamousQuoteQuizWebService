using System.ComponentModel.DataAnnotations;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Server.Api.BindingModels.Modes
{
    public class QuizModeBindingModel
    {
        [Required]
        public QuizModeType Type { get; set; }
    }
}