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
        public DateTime DateAsked { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();       
        public List<QuestionsTags> QuestionsTags { get; set; } = new List<QuestionsTags>();
        public List<Likes> Likes { get; set; } = new List<Likes>();
    }
}
