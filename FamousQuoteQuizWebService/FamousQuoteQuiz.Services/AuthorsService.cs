using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using FamousQuoteQuiz.Data.Repositories;
using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.Services.DTOs;
using FamousQuoteQuiz.Utils;
using FamousQuoteQuiz.Data;

namespace FamousQuoteQuiz.Services
{
    public class AuthorsService : IAuthorsService
    {
        private IRepository<Author> authorsRepository;

        public AuthorsService(IRepository<Author> authorsRepository)
        {
            this.authorsRepository = authorsRepository;
        }

        public async Task<AuthorDTO> GetRandomAuthor()
        {
            int authorsCount = this.authorsRepository.GetAll().Count();

            if (authorsCount == 0)
            {
                throw new UnpopulatedDbException(
                    "No authors in the database. Please populate db with authors first.");
            }

            int randomAuthorId = StaticRandomizer.RandomNumber(1, authorsCount + 1);

            var randomAuthor = await this.authorsRepository
                                        .GetAll()
                                        .Where(x => x.Id == randomAuthorId)
                                        .Select(AuthorDTO.MapToDTO)
                                        .FirstOrDefaultAsync();

            return randomAuthor;
        }
    }
}
