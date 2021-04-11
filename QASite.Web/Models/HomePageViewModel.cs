using QASite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QASite.Web.Models
{
    public class HomePageViewModel
    {
        public List<Question> Questions { get; set; }   
    }
    public class ViewQuestionViewModel
    {
        public Question Question { get; set; }
        public bool IsLiked { get; set; }
    }
    public class QuestionsTagViewModel
    {
        public List<Question> Questions { get; set; }
        public string Name { get; set; }
    }
}
