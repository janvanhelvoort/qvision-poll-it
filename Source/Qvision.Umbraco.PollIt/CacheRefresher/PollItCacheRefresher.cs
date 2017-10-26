namespace Qvision.PollIt.CacheRefresher
{
    using System;

    using Qvision.PollIt.Constants;

    using Umbraco.Core.Cache;
    using Umbraco.Web.Cache;

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
