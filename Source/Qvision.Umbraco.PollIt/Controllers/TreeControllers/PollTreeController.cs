namespace Qvision.PollIt.Controllers.TreeControllers
{
    using System.Globalization;
    using System.Net.Http.Formatting;

    using Qvision.PollIt.Constants;
    using Qvision.PollIt.Models.Repositories;

    using umbraco.BusinessLogic.Actions;

    using Umbraco.Core;
    using Umbraco.Web.Models.Trees;
    using Umbraco.Web.Mvc;
    using Umbraco.Web.Trees;

    [Tree(ApplicationConstants.SectionAlias, TreeConstants.TreeAlias, TreeConstants.TreeTitle)]
    [PluginController(ApplicationConstants.SectionAlias)]
    public class PollTreeController : TreeController
    {
        private readonly string rootId = Constants.System.Root.ToString(CultureInfo.InvariantCulture);

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            TreeNodeCollection nodes = new TreeNodeCollection();

            if (id.Equals(this.rootId))
            {
                var questionRespository = new QuestionRepository();
                var questions = questionRespository.Get();

                foreach (var question in questions)
                {
                    nodes.Add(this.CreateTreeNode(question.Id.ToString(), id, queryStrings, question.Name, "icon-poll", false));
                }
            }

            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            MenuItemCollection collection = new MenuItemCollection();

            if (id.Equals(this.rootId))
            {
                collection.Items.Add<CreateChildEntity, ActionNew>(this.Localize("actions/" + ActionNew.Instance.Alias));
            }
            else
            {
                collection.Items.Add<ActionDelete>(this.Localize("actions/" + ActionDelete.Instance.Alias));
            }

            collection.Items.Add<RefreshNode, ActionRefresh>(this.Localize("actions/" + ActionRefresh.Instance.Alias));

            return collection;
        }

        private string Localize(string key)
        {
            return this.ApplicationContext.Services.TextService.Localize(key, CultureInfo.CurrentCulture);
        }
    }
}
