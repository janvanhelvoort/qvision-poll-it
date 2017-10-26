namespace Qvision.PollIt.Models.Pocos
{
    using System;
    using System.Collections.Generic;

    using Qvision.PollIt.Constants;

    using Umbraco.Core.Persistence;

    /// <summary>
    /// The poll question.
    /// </summary>
    [TableName(TableConstants.Questions.TableName)]    
    public class Question
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>        
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>                
        public DateTime CreatedDate { get; set; }

        [Ignore]
        public IEnumerable<Answer> Answers { get; set; }

        [Ignore]
        public IEnumerable<Response> Responses { get; set; }
    }
}
