namespace Qvision.Umbraco.PollIt.Controllers.ApiControllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using global::Umbraco.Core;
    using global::Umbraco.Web.Editors;

    using Qvision.Umbraco.PollIt.Attributes;
    using Qvision.Umbraco.PollIt.Constants;
    using Qvision.Umbraco.PollIt.Models.Pocos;
    using Qvision.Umbraco.PollIt.Models.Repositories;

    [CamelCase]
    public class AnswerApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public HttpResponseMessage Post(Answer answer)
        {
            var result = AnswerRepository.Current.Save(answer);

            if (result != null)
            {
                ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch($"{RuntimeCacheConstants.RuntimeCacheKeyPrefix}{answer.QuestionId}");
                this.Request.CreateResponse(HttpStatusCode.OK, answer);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Answer can't save");
        }

        [HttpPost]
        public HttpResponseMessage UpdateSort(int[] ids)
        {
            var answer = AnswerRepository.Current.GetById(ids.FirstOrDefault());

            if (ids != null && ids.Length > 0)
            {
                for (var i = 0; i < ids.Length; ++i)
                {
                    AnswerRepository.Current.UpdateSort(ids[i], i);
                }
            }

            ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch($"{RuntimeCacheConstants.RuntimeCacheKeyPrefix}{answer.QuestionId}");

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var answer = AnswerRepository.Current.GetById(id);

            using (var transaction = this.ApplicationContext.DatabaseContext.Database.GetTransaction())
            {
                if (ResponseRepository.Current.DeleteByAnswerId(id) && AnswerRepository.Current.Delete(id))
                {
                    transaction.Complete();
                    ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch($"{RuntimeCacheConstants.RuntimeCacheKeyPrefix}{answer.QuestionId}");

                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Answer can't delete");
        }
    }
}