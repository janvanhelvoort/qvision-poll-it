namespace Qvision.Umbraco.PollIt.Controllers.ApiControllers
{    
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using global::Umbraco.Web.Editors;

    using Qvision.Umbraco.PollIt.Attributes;
    using Qvision.Umbraco.PollIt.Models.Pocos;
    using Qvision.Umbraco.PollIt.Models.Repositories;
    using Qvision.Umbraco.PollIt.CacheRefresher;
    

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
                this.Request.CreateResponse(HttpStatusCode.OK, answer);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Can't save answer");
        }

        [HttpPost]
        public HttpResponseMessage UpdateSort(int[] ids, int questionId)
        {
            if (ids != null && ids.Length > 0)
            {
                for (var i = 0; i < ids.Length; ++i)
                {
                    AnswerRepository.Current.UpdateSort(ids[i], i);
                }
            }

            PollItCacheRefresher.ClearCache(questionId);

            return this.Request.CreateResponse(HttpStatusCode.OK);
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