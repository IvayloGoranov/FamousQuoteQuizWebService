using System.Web.Http;
using System.Threading.Tasks;

using FamousQuoteQuiz.Services;
using FamousQuoteQuiz.Server.Infrastructure.Attributes;
using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Server.Api.BindingModels.Modes;

namespace FamousQuoteQuiz.Server.Api.Controllers
{
    [SessionAuthorize]
    public class ModesController : ApiController
    {
        private IModesService modesService;

        public ModesController(IModesService modesService)
        {
            this.modesService = modesService;
        }

        // GET api/modes
        [HttpGet]
        [Route("api/modes")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSelectedMode()
        {
            try
            {
                var selectedMode = await this.modesService.GetSelectedMode();

                return this.Ok(selectedMode);
            }
            catch (UnpopulatedDbException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        // POST api/modes
        [HttpPost]
        [Route("api/modes")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody]QuizModeBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.modesService.UpdateMode(model.Type);

            return this.Ok();
        }
    }
}
