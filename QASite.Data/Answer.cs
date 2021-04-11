using System;
using System.Collections.Generic;
using System.Text;

namespace QASite.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Question Question { get; set; }
    }
}
