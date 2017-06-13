using System.Threading.Tasks;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Services
{
    public interface IModesService
    {
        Task<QuizModeType> GetSelectedMode();

        Task<int> UpdateMode(QuizModeType newType);
    }
}
