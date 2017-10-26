namespace Qvision.PollIt.Models.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Qvision.PollIt.Constants;
    using Qvision.PollIt.Models.Pocos;

    using Umbraco.Core;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.SqlSyntax;

    internal class QuestionRepository
    {
        public static readonly QuestionRepository Current = new QuestionRepository();

        public Database Database => ApplicationContext.Current.DatabaseContext.Database;

        public ISqlSyntaxProvider SqlSyntax => ApplicationContext.Current.DatabaseContext.SqlSyntax;

        public IEnumerable<Question> Get()
        {
            var query = new Sql().Select("*").From(TableConstants.Questions.TableName);

            return this.Database.Fetch<Question>(query);
        }

        public Question GetById(int id)
        {
            var query = new Sql().Select("*").From(TableConstants.Questions.TableName).Where<Question>(x => x.Id.Equals(id), this.SqlSyntax);

            return this.Database.Fetch<Question>(query).FirstOrDefault();
        }

        public Question Save(Question question)
        {
            if (question != null)
            {
                if (question.Id > 0)
                {
                    this.Database.Update(question);
                }
                else
                {
                    question.CreatedDate = DateTime.Now;
                    this.Database.Save(question);
                }
            }

            return question;
        }

        public bool Delete(int id)
        {
            var result = this.Database.Delete<Question>(id);

            return result > 0;
        }

        public IEnumerable<Answer> GetAnswers(int id)
        {
            var query = new Sql().Select("*").From(TableConstants.Answers.TableName).Where<Answer>(x => x.QuestionId.Equals(id), this.SqlSyntax);

            return this.Database.Fetch<Answer>(query);
        }

        public Answer PostAnswer(int id, Answer answer)
        {
            if (answer != null)
            {
                answer.QuestionId = id;
                this.Database.Save(answer);
            }

            return answer;
        }

        public IEnumerable<Response> GetResponses(int id)
        {
            var query = new Sql().Select("*").From(TableConstants.Responses.TableName).Where<Response>(x => x.QuestionId.Equals(id), this.SqlSyntax);

            return this.Database.Fetch<Response>(query);
        }

        public Response PostResponse(int id, int answerId)
        {
            var response = new Response { QuestionId = id, AnswerId = answerId, ResponseDate = DateTime.Now.Date };

            this.Database.Save(response);

            return response;
        }
    }
}
