namespace Qvision.PollIt.Controllers.ApiControllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Qvision.PollIt.Attributes;
    using Qvision.PollIt.CacheRefresher;
    using Qvision.PollIt.Models.Pocos;
    using Qvision.PollIt.Models.Repositories;

    using Umbraco.Web.Editors;

    [CamelCase]
    public class AnswerApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public HttpResponseMessage Post(Answer answer)
        {
            var result = AnswerRepository.Current.Save(answer);

            if (result != null)
            {
                PollItCacheRefresher.ClearCache(answer.QuestionId);
                return this.Request.CreateResponse(HttpStatusCode.OK, answer);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't save answer");
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id, int questionId)
        {
            using (var transaction = this.ApplicationContext.DatabaseContext.Database.GetTransaction())
            {
                if (ResponseRepository.Current.DeleteByAnswerId(id) && AnswerRepository.Current.Delete(id))
                {
                    transaction.Complete();
                    PollItCacheRefresher.ClearCache(questionId);

                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't delete answer");
        }
    }
}