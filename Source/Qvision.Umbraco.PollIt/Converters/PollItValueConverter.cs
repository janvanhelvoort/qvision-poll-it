namespace Qvision.PollIt.Converters
{
    using System;

    using Qvision.PollIt.Constants;
    using Qvision.PollIt.Models;
    using Qvision.PollIt.Services;

    using Umbraco.Core.Logging;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;

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
