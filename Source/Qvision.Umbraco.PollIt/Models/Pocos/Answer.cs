namespace Qvision.PollIt.Models.Pocos
{
    using Qvision.PollIt.Constants;

    using Umbraco.Core.Persistence;

    /// <summary>
    /// The answer.
    /// </summary>
    [TableName(TableConstants.Answers.TableName)]
    public class Answer
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        public int Index { get; set; }
    }
}