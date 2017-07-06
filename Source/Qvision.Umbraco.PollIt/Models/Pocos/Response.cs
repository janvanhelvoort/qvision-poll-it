namespace Qvision.Umbraco.PollIt.Models.Pocos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.DatabaseAnnotations;

    using Qvision.Umbraco.PollIt.Constants;

    /// <summary>
    /// The poll response.
    /// </summary>
    [TableName(TableConstants.Responses.TableName)]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Response
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [PrimaryKeyColumn(AutoIncrement = true)]
        [Required, DataMember(IsRequired = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the poll id.
        /// </summary>
        [ForeignKey(typeof(Question), Name = "QuestionId_Response")]
        [Index(IndexTypes.NonClustered, Name = "QuestionId_Response")]
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        [ForeignKey(typeof(Answer), Name = "AnswerId_Response")]
        [Index(IndexTypes.NonClustered, Name = "AnswerId_Response")]
        public int AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the response date.
        /// </summary>        
        [Required, DataMember(IsRequired = true)]
        public DateTime ResponseDate { get; set; }
    }
}