namespace Qvision.Umbraco.PollIt.Models.Pocos
{
    using System;

    using global::Umbraco.Core.Persistence;

    using Qvision.Umbraco.PollIt.Constants;

    /// <summary>
    /// The poll response.
    /// </summary>
    [TableName(TableConstants.Responses.TableName)]
    public class Response
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the poll id.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        public int AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the response date.
        /// </summary>        
        public DateTime ResponseDate { get; set; }
    }
}