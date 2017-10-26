namespace Qvision.PollIt.Controllers.ApiControllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Qvision.PollIt.Attributes;
    using Qvision.PollIt.Models.Repositories;

    using Umbraco.Web.Editors;

    [CamelCase]
    public class OverviewApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var questions = QuestionRepository.Current.Get().ToList();
            var answers = AnswerRepository.Current.Get().ToList();
            var responses = ResponseRepository.Current.Get().ToList();

            foreach (var question in questions)
            {
                question.Answers = answers.Where(answer => answer.QuestionId.Equals(question.Id));
                question.Responses = responses.Where(response => response.QuestionId.Equals(question.Id));
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, questions);
        }
    }
}