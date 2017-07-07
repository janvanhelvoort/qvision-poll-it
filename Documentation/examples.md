# Examples
Small example of a controller and view.

## Controller
```csharp
    public class PollSurfaceController : SurfaceController
    {
        public ActionResult Get(int id)
        {
            var poll = PollItService.Current.GetQuestion(id);
            return this.View(poll);
        }

        public ActionResult Post([FromBody] int questionId, [FromBody] int answerId)
        {
            var poll = PollItService.Current.Vote(questionId, answerId);
            return this.View("partials/poll", poll);
        }
    }
```

## View
```csharp
@inherits UmbracoViewPage<Qvision.Umbraco.PollIt.Models.Question>

<p>@Model.Name</p>
<ul>
    @using (Html.BeginUmbracoForm("Post", "PollSurface"))
    {
        @Html.Hidden("questionId", Model.Id)

        foreach (var answer in Model.Answers)
        {
            <li>
                @Html.RadioButton("answerId", answer.Id, false)
                @answer.Percentage% - @answer.Value
            </li>
        }

        <button type="submit">post</button>
    }
</ul>
```
