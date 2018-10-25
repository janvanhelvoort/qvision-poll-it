namespace Qvision.PollIt.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Qvision.PollIt.CacheRefresher;
    using Qvision.PollIt.Constants;
    using Qvision.PollIt.Models;
    using Qvision.PollIt.Models.Repositories;

    using Umbraco.Core;

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
                    var answerResponses = Mapper.Map<IEnumerable<Response>>(responses.Where(item => item.AnswerId.Equals(answer.Id))).ToList();

                    answer.Responses = answerResponses;
                    answer.Percentage = answerResponses.Any() ? Math.Round((double)(answerResponses.Count) / responses.Count * 100) : 0;
                }

                return question;
            }, TimeSpan.FromMinutes(RuntimeCacheConstants.DefaultExpiration), true);
        }

        public Question Vote(int questionId, int answerId)
        {
            var result = QuestionRepository.Current.PostResponse(questionId, answerId);

            if (result != null)
            {
                PollItCacheRefresher.ClearCache(questionId);
            }

            return this.GetQuestion(questionId);
        }
    }
}
