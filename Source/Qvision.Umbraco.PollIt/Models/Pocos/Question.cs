namespace Qvision.Umbraco.PollIt.Models.Pocos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.DatabaseAnnotations;

    using Qvision.Umbraco.PollIt.Constants;

    /// <summary>
    /// The poll question.
    /// </summary>
    [TableName(TableConstants.Questions.TableName)]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Question
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [PrimaryKeyColumn(AutoIncrement = true)]
        [Required, DataMember(IsRequired = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>        
        [Required, DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [DataMember(IsRequired = false)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>        
        [DataMember(IsRequired = false)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>        
        [Required, DataMember(IsRequired = true)]
        public DateTime CreatedDate { get; set; }

        [Ignore]
        public IEnumerable<Answer> Answers { get; set; }

        [Ignore]
        public IEnumerable<Response> Responses { get; set; }
    }
}
