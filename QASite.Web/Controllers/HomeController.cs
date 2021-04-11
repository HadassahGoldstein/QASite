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
            var ids = HttpContext.Session.Get<List<int>>("like-ids");
            var vm = new ViewQuestionViewModel()
            {
                Question = repo.GetQuestionById(id),
                IsLiked = ids == null ? false : ids.Contains(id)
            };
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
            q.Questioner = repo.GetByEmail(User.Identity.Name).Name;
            repo.AddQuestion(q, tags);
            return Redirect("/");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer a)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            User u = repo.GetByEmail(User.Identity.Name);
            a.Name = u.Name;
            a.Date = DateTime.Now;
            repo.AddAnswer(a);
            return Redirect($"/home/selectQuestion?id={a.QuestionId}");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddLike(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            var ids = HttpContext.Session.Get<List<int>>("like-ids");
            if (ids == null)
            {
                ids = new List<int>();
            }
            if (ids.Contains(id))
            {
                return Json(id);
            }
            repo.AddLike(id);
            ids.Add(id);
            HttpContext.Session.Set("like-ids", ids);
            return Json(id);
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
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
