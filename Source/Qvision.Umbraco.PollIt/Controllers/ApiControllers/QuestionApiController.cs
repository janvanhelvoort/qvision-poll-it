namespace Qvision.PollIt.Controllers.ApiControllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Qvision.PollIt.Attributes;
    using Qvision.PollIt.CacheRefresher;
    using Qvision.PollIt.Models.Pocos;
    using Qvision.PollIt.Models.Repositories;

    using Umbraco.Web.Editors;

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
            using (var transaction = this.ApplicationContext.DatabaseContext.Database.GetTransaction())
            {
                // add or update question
                question = QuestionRepository.Current.Save(question);

                if (question != null)
                {
                    // remove old answers, they don't appear in the result.answers array
                    var oldAnswers = QuestionRepository.Current.GetAnswers(question.Id).Where(a => !question.Answers.Any(r => r.Id.Equals(a.Id)));
                    foreach (var deletedAnswer in oldAnswers)
                    {
                        if (!ResponseRepository.Current.DeleteByAnswerId(deletedAnswer.Id) && !AnswerRepository.Current.Delete(deletedAnswer.Id))
                        {
                            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't delete old answers, Error add of update of the quesion");
                        }
                    }

                    // add or update answers
                    foreach (var answer in question.Answers)
                    {
                        var result = answer.Id != 0 ? AnswerRepository.Current.Save(answer) : QuestionRepository.Current.PostAnswer(question.Id, answer);

                        if (result == null)
                        {
                            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't add answer to question");
                        }
                    }

                    transaction.Complete();
                    PollItCacheRefresher.ClearCache(question.Id);

                    return this.Request.CreateResponse(HttpStatusCode.OK, question);
                }

                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't save question");
            }
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
                        return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't delete question, Error removing the responses");
                    }
                }

                foreach (var answer in answers)
                {
                    if (!AnswerRepository.Current.Delete(answer.Id))
                    {
                        return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't delete question, Error removing the answers");
                    }
                }

                if (QuestionRepository.Current.Delete(id))
                {
                    transaction.Complete();

                    PollItCacheRefresher.ClearCache(id);

                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't delete question");
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

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't add answer to question");
        }

        [HttpGet]
        public HttpResponseMessage GetResponses(int id)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, QuestionRepository.Current.GetResponses(id));
        }
    }
}
