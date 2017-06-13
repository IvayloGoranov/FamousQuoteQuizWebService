using System.Web.Http;
using System.Threading.Tasks;

using FamousQuoteQuiz.Services;
using FamousQuoteQuiz.Server.Infrastructure.Attributes;
using FamousQuoteQuiz.Data;

namespace FamousQuoteQuiz.Server.Api.Controllers
{
    [SessionAuthorize]
    public class AuthorsController : ApiController
    {
        private IAuthorsService authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            this.authorsService = authorsService;       
        }

        // GET api/authors
        [HttpGet]
        [Route("api/authors")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetRandomAuthor()
        {
            try
            {
                var randomAuthor = await this.authorsService.GetRandomAuthor();

                return this.Ok(randomAuthor);
            }
            catch (UnpopulatedDbException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
