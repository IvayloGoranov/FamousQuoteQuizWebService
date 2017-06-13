using System.Threading.Tasks;

using FamousQuoteQuiz.Services.DTOs;

namespace FamousQuoteQuiz.Services
{
    public interface IAuthorsService
    {
        Task<AuthorDTO> GetRandomAuthor();
    }
}
