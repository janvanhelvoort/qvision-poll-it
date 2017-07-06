namespace Qvision.Umbraco.PollIt.Models.Pocos
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.DatabaseAnnotations;

    using Qvision.Umbraco.PollIt.Constants;

    /// <summary>
    /// The answer.
    /// </summary>
    [TableName(TableConstants.Answers.TableName)]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Answer
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [PrimaryKeyColumn(AutoIncrement = true)]
        [Required, DataMember(IsRequired = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        [ForeignKey(typeof(Question), Name = "QuestionId_Answer")]
        [Index(IndexTypes.NonClustered, Name = "QuestionId_Answer")]
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>        
        [Required, DataMember(IsRequired = true)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        [Required, DataMember(IsRequired = true)]
        public int Index { get; set; }
    }
}