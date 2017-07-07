namespace Qvision.Umbraco.PollIt.Converters
{
    using System;

    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models.PublishedContent;
    using global::Umbraco.Core.PropertyEditors;

    using Qvision.Umbraco.PollIt.Constants;
    using Qvision.Umbraco.PollIt.Models;
    using Qvision.Umbraco.PollIt.Services;

    [PropertyValueType(typeof(Question))]
    public class PollItValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return ApplicationConstants.PropertyEditorAlias.Equals(propertyType.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(source?.ToString()))
                {
                    int id;

                    if (int.TryParse(source.ToString(), out id))
                    {
                        return PollItService.Current.GetQuestion(id);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error<PollItValueConverter>("Error converting value", e);
            }

            return null;
        }
    }
}
