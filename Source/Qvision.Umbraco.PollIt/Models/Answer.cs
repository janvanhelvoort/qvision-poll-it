﻿namespace Qvision.Umbraco.PollIt.Models
{
    using System.Collections.Generic;  

    public class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int Index { get; set; }

        public IEnumerable<Response> Responses { get; set; }
    }
}