namespace Qvision.Umbraco.PollIt.CacheRefresher
{
    using System;

    using global::Umbraco.Core.Cache;
    using global::Umbraco.Web.Cache;

    using Qvision.Umbraco.PollIt.Constants;

    public class PollItCacheRefresher : JsonCacheRefresherBase<PollItCacheRefresher>
    {
        protected override PollItCacheRefresher Instance
        {
            get { return this; }
        }

        public override Guid UniqueIdentifier
        {
            get { return new Guid(CacheRefresherConstants.PollItCacheRefreshId); }
        }

        public override string Name
        {
            get { return "Poll-it cache refresher"; }
        }

        public static void ClearCache(int questionId = 0)
        {
            DistributedCache.Instance.Refresh(new Guid(CacheRefresherConstants.PollItCacheRefreshId), questionId);
        }
    }
}
