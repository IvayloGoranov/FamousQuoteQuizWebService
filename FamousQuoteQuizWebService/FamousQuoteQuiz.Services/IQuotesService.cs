using System.Threading.Tasks;

using FamousQuoteQuiz.Services.DTOs;

namespace FamousQuoteQuiz.Services
{
    public interface IQuotesService
    {
        Task<QuoteDTO> GetRandomQuote();

        Task<QuoteDTO> GetQuoteById(int id);
    }
}
