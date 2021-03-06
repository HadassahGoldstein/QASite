using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QASite.Data
{
    public class QARepository
    {
        private readonly string _connectionString;
        public QARepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User u,string password)
        {
            u.PasswordHash= BCrypt.Net.BCrypt.HashPassword(password);
            using var context = new QADbContext(_connectionString);
            context.Users.Add(u);
            context.SaveChanges();
        }
        public bool Login(string email,string password)
        {
            using var context = new QADbContext(_connectionString);
            User user=context.Users.FirstOrDefault(u=>u.Email==email);
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            
        }
        public User GetByEmail(string email)
        {
            using var context = new QADbContext(_connectionString);
            User user = context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }
        public List<Question> GetQuestions()
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions
                .Include(q=>q.User)
                .Include(q=>q.QuestionsTags)
                .ThenInclude(qt=>qt.Tag)
                .Include(q=>q.Answers)
                .ThenInclude(a=>a.User)
                .Include(q=>q.Likes)
                .OrderByDescending(q=>q.DateAsked).ToList();           
        }
        public Question GetQuestionById(int id)
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions
                .Include(q=>q.User)
                .Include(q => q.Answers)
                .ThenInclude(a=>a.User)
                .Include(q => q.QuestionsTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q=>q.Likes)
                .FirstOrDefault(q => q.Id == id);
        }
        public void AddAnswer(Answer a)
        {
            using var context = new QADbContext(_connectionString);
            context.Answers.Add(a);
            context.SaveChanges();
        }
        public void AddQuestion(Question q,List<string> tags)
        {
            using var context = new QADbContext(_connectionString);
            
            context.Questions.Add(q);
            context.SaveChanges();
            foreach(string name in tags)
            {
                Tag t = GetTag(name);
                int tagId;
                if (t == null)
                {
                   tagId= AddTag(name);
                }
                else 
                {
                    tagId = t.Id;
                }
                context.QuestionsTags.Add(new QuestionsTags() { QuestionId = q.Id, TagId = tagId });
            }
            context.SaveChanges();
        }
        private Tag GetTag(string name)
        {
            using var context = new QADbContext(_connectionString);
            return context.Tags.FirstOrDefault(t => t.Name == name);
        }
        private int AddTag(string name)
        {
            using var context = new QADbContext(_connectionString);
            Tag t = new Tag() { Name = name };
            context.Tags.Add(t);
            context.SaveChanges();
            return t.Id;
        }
        public void AddLike(int questionId,int userId)
        {
            using var context = new QADbContext(_connectionString);
            context.Likes.Add(new Likes { QuestionId = questionId, UserId = userId });
            context.SaveChanges();
        }
        public int GetCurrentLikes(int id)
        {
            using var context = new QADbContext(_connectionString);
            return context.Likes.Count(l=> l.QuestionId == id);            
        }       
        public List<Question> GetQuestionsForTag(string name)
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions.Include(q => q.QuestionsTags)             
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.Answers)
                .Where(q=>q.QuestionsTags.Any(qt=>qt.Tag.Name==name))
                .Include(q => q.User)
                .Include(q => q.Likes)
                .OrderByDescending(q => q.DateAsked).ToList();
        }           
    }
}
