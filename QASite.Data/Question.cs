using System;
using System.Collections.Generic;
using System.Text;

namespace QASite.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        public DateTime DateAsked { get; set; }
        public string Questioner { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();       
        public List<QuestionsTags> QuestionsTags { get; set; } = new List<QuestionsTags>();
    }
}
