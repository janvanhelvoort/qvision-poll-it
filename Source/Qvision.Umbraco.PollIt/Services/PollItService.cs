namespace Qvision.Umbraco.PollIt.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using global::Umbraco.Core;    

    using Qvision.Umbraco.PollIt.Constants;
    using Qvision.Umbraco.PollIt.Models;    
    using Qvision.Umbraco.PollIt.Models.Repositories;

    public class PollItService
    {
        public static readonly PollItService Current = new PollItService();

        public Question GetQuestion(int id)
        {
            return (Question)ApplicationContext.Current.ApplicationCache.RuntimeCache.GetCacheItem($"{RuntimeCacheConstants.RuntimeCacheKeyPrefix}{id}", () =>
            {
                var question = Mapper.Map<Question>(QuestionRepository.Current.GetById(id));
                question.Answers = Mapper.Map<IEnumerable<Answer>>(QuestionRepository.Current.GetAnswers(id).OrderBy(i => i.Index));

                var responses = QuestionRepository.Current.GetResponses(id).ToList();

                question.Responses = responses.Count;

                foreach (var answer in question.Answers)
                {                    
                    answer.Responses = Mapper.Map<IEnumerable<Response>>(responses.Where(item => item.AnswerId.Equals(answer.Id)));
                }

                return question;
            }, TimeSpan.FromMinutes(RuntimeCacheConstants.DefaultExpiration), true);
        }

        public Question Vote(int questionId, int answerId)
        {
            var result = QuestionRepository.Current.PostResponse(questionId, answerId);

            if (result != null)
            {
                ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch($"{RuntimeCacheConstants.RuntimeCacheKeyPrefix}{questionId}");
            }            

            return this.GetQuestion(questionId);
        }
    }
}
