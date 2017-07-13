namespace Qvision.Umbraco.PollIt.Controllers.ApiControllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using global::Umbraco.Web.Editors;

    using Qvision.Umbraco.PollIt.Attributes;
    using Qvision.Umbraco.PollIt.CacheRefresher;
    using Qvision.Umbraco.PollIt.Models.Pocos;
    using Qvision.Umbraco.PollIt.Models.Repositories;

    [CamelCase]
    public class QuestionApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, QuestionRepository.Current.Get());
        }

        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            var result = QuestionRepository.Current.GetById(id);

            return result == null ? this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Question not found") : this.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage Post(Question question)
        {
            var result = QuestionRepository.Current.Save(question);

            if (result != null)
            {
                PollItCacheRefresher.ClearCache(result.Id);
                
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            
            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Question can't save");
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            using (var transaction = this.ApplicationContext.DatabaseContext.Database.GetTransaction())
            {
                var responses = QuestionRepository.Current.GetResponses(id);
                var answers = QuestionRepository.Current.GetAnswers(id);

                foreach (var response in responses)
                {
                    if (!ResponseRepository.Current.Delete(response.Id))
                    {
                        return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Question can't delete, Error removing of the responses");
                    }
                }

                foreach (var answer in answers)
                {
                    if (!AnswerRepository.Current.Delete(answer.Id))
                    {
                        return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Question can't delete, Error removing of the answers");
                    }
                }

                if (QuestionRepository.Current.Delete(id))
                {
                    transaction.Complete();

                    PollItCacheRefresher.ClearCache(id);

                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Question can't delete");
        }

        [HttpGet]
        public HttpResponseMessage GetAnswers(int id)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, QuestionRepository.Current.GetAnswers(id));
        }

        [HttpPost]
        public HttpResponseMessage PostAnswer(int id, Answer answer)
        {
            var result = QuestionRepository.Current.PostAnswer(id, answer);

            if (result != null)
            {
                PollItCacheRefresher.ClearCache(id);

                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't add answer");
        }

        [HttpGet]
        public HttpResponseMessage GetResponses(int id)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, QuestionRepository.Current.GetResponses(id));
        }
    }
}
