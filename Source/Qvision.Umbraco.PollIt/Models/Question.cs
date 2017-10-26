namespace Qvision.PollIt.Models
{
    using System;
    using System.Collections.Generic;

    public class Question
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Responses { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; }       

        public IEnumerable<Answer> Answers { get; set; }        
    }
}

