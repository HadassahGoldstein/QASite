using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QASite.Data;
using QASite.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QASite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            var vm = new HomePageViewModel()
            {
                Questions = repo.GetQuestions(),
            };
            return View(vm);
        }
        public IActionResult SelectQuestion(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            Question q = repo.GetQuestionById(id);
            User u = repo.GetByEmail(User.Identity.Name);
            var vm = new ViewQuestionViewModel()
            {
               Question=q                               
            };
            if (u != null)
            {
                vm.IsLiked = q.Likes.Any(l =>l.UserId == u.Id);
            }
            return View(vm);
        }
        [Authorize]
        public IActionResult NewQuestion()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult NewQuestion(Question q, List<string> tags)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            q.UserId = repo.GetByEmail(User.Identity.Name).Id;
            repo.AddQuestion(q, tags);
            return Redirect("/");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer a)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            a.UserId = repo.GetByEmail(User.Identity.Name).Id;            
            a.Date = DateTime.Now;
            repo.AddAnswer(a);
            return Redirect($"/home/selectQuestion?id={a.QuestionId}");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddLike(int questionId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            User u = repo.GetByEmail(User.Identity.Name);
            repo.AddLike(questionId, u.Id);                                              
            return Json(questionId);
        }
        public IActionResult CurrentLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            int likes = repo.GetCurrentLikes(id);
            return Json(likes);
        }
        public IActionResult QuestionsForTag(string name)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            QuestionsTagViewModel vm = new QuestionsTagViewModel()
            {
                Questions = repo.GetQuestionsForTag(name),
                Name = name
            };
            return View(vm);
        }
    }
    
}
