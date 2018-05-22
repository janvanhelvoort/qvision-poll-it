namespace Qvision.PollIt
{
    using System.Configuration;

    using Umbraco.Core;

    /// <summary>
    /// The Poll-it Context
    /// </summary>
    public class PollItContext
    {
        /// <summary>
        /// The current instance.
        /// </summary>
        private static PollItContext instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="PollItContext"/> class from being created.
        /// </summary>
        private PollItContext()
        {
            this.AmountOfAnswers = this.GetAppSetting<int>(Constants.AppSettings.AmountOfAnswers, 6);
            instance = this;
        }

        /// <summary>
        /// Gets the current context
        /// </summary>
        public static PollItContext Current => instance ?? new PollItContext();

        // Amount of answers
        public int AmountOfAnswers { get; set; }

        /// <summary>
        /// Gets the value of app setting 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="T">The return type</typeparam>
        /// <returns>The <see cref="T"/>.</returns>
        private T GetAppSetting<T>(string key, T defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[key];

            if (setting != null)
            {
                var attempConvert = setting.TryConvertTo<T>();

                if (attempConvert.Success)
                {
                    return attempConvert.Result;
                }
            }

            return defaultValue;
        }
    }
}
