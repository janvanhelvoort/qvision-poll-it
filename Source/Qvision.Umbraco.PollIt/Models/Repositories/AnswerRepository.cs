namespace Qvision.PollIt.Models.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using Qvision.PollIt.Constants;
    using Qvision.PollIt.Models.Pocos;

    using Umbraco.Core;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.SqlSyntax;

    internal class AnswerRepository
    {
        public static readonly AnswerRepository Current = new AnswerRepository();

        public Database Database => ApplicationContext.Current.DatabaseContext.Database;

        public ISqlSyntaxProvider SqlSyntax => ApplicationContext.Current.DatabaseContext.SqlSyntax;

        public IEnumerable<Answer> Get()
        {
            var query = new Sql().Select("*").From(TableConstants.Answers.TableName);

            return this.Database.Fetch<Answer>(query);
        }

        public Answer GetById(int id)
        {
            var query = new Sql().Select("*").From(TableConstants.Answers.TableName).Where<Answer>(x => x.Id.Equals(id), this.SqlSyntax);

            return this.Database.Fetch<Answer>(query).FirstOrDefault();
        }

        public Answer Save(Answer answer)
        {
            if (answer != null && answer.Id > 0)
            {
                this.Database.Update(answer);
            }

            return answer;
        }

        public bool Delete(int id)
        {
            var result = this.Database.Delete<Answer>(id);

            return result > 0;
        }
    }
}