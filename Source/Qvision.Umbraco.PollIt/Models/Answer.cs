namespace Qvision.PollIt.Models
{
    using System.Collections.Generic;

    public class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int Index { get; set; }

        public double Percentage { get; set; }

        public IEnumerable<Response> Responses { get; set; }
    }
}