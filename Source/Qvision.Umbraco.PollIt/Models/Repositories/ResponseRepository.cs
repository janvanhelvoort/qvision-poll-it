namespace Qvision.PollIt.Models.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using Qvision.PollIt.Constants;
    using Qvision.PollIt.Models.Pocos;

    using Umbraco.Core;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.SqlSyntax;

    internal class ResponseRepository
    {
        public static readonly ResponseRepository Current = new ResponseRepository();

        public Database Database => ApplicationContext.Current.DatabaseContext.Database;

        public ISqlSyntaxProvider SqlSyntax => ApplicationContext.Current.DatabaseContext.SqlSyntax;

        public IEnumerable<Response> Get()
        {
            var query = new Sql().Select("*").From(TableConstants.Responses.TableName);

            return this.Database.Fetch<Response>(query);
        }

        public Response GetById(int id)
        {
            var query = new Sql().Select("*").From(TableConstants.Responses.TableName).Where<Response>(x => x.Id.Equals(id), this.SqlSyntax);

            return this.Database.Fetch<Response>(query).FirstOrDefault();
        }

        public bool Delete(int id)
        {
            var result = this.Database.Delete<Response>(id);

            return result > 0;
        }

        public bool DeleteByAnswerId(int id)
        {
            var query = new Sql().Where<Response>(response => response.AnswerId.Equals(id), this.SqlSyntax);

            return this.Database.Delete<Response>(query) > -1;
        }
    }
}