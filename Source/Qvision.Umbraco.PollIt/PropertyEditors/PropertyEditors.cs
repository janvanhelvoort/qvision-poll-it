namespace Qvision.Umbraco.PollIt
{
    using global::Umbraco.Core.PropertyEditors;

    using Qvision.Umbraco.PollIt.Constants;

    /// <summary>
    /// Poll-it property editor.
    /// </summary>
    [PropertyEditor(ApplicationConstants.PropertyEditorAlias, "Poll It Picker", "~/App_Plugins/pollit/backoffice/propertyEditors/picker.html", Group = "Pickers", Icon = "icon-poll", ValueType = "JSON")]
    public class PollItPropertyEditor : PropertyEditor
    {
    }
}
