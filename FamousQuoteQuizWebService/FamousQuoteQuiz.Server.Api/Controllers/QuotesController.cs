using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

using FamousQuoteQuiz.Services;
using FamousQuoteQuiz.Server.Infrastructure.Attributes;
using FamousQuoteQuiz.Data;

namespace FamousQuoteQuiz.Server.Api.Controllers
{
    [SessionAuthorize]
    public class QuotesController : ApiController
    {
        private IQuotesService quotesService;

        public QuotesController(IQuotesService quotesService)
        {
            this.quotesService = quotesService;
        }
        
        // GET api/quotes
        [HttpGet]
        [Route("api/quotes")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetRandomQuote()
        {
            try
            {
                var randomQuote = await this.quotesService.GetRandomQuote();

                return this.Ok(randomQuote);
            }
            catch (UnpopulatedDbException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        // GET api/quotes/id
        [HttpGet]
        [Route("api/quotes/{id:int}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetQuoteById(int id)
        {
            try
            {
                var quote = await this.quotesService.GetQuoteById(id);

                return this.Ok(quote);
            }
            catch (KeyNotFoundException ex)
            {
                return this.Content(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
