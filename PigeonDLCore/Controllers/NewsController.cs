using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PigeonDLCore.Data;
using PigeonDLCore.Repository;
using System.Security.Claims;

namespace PigeonDLCore.Controllers
{
    public class NewsController : Controller
    {
        private Repository.NewsRepository _repository;

        public NewsController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.NewsRepository(dbContext);
        }

        // GET: NewsController
        public ActionResult Index()
        {
            var news = _repository.GetAllNews();
            return View("Index", news);
        }

        // GET: NewsController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetNewsByID(id);
            ViewData["PageTitle"] = model.Title;
            return View("Details", model);
        }

        // GET: NewsController/Create
        [Authorize(Roles = "Admin, Owner")]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Owner")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.News model = new Models.News();
                var task = TryUpdateModelAsync(model);
                task.Wait();

                model.IDUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _repository.InsertNews(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewsController/Edit/5
        [Authorize(Roles = "Admin, Owner")]
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetNewsByID(id);
            return View("Edit", model);
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Owner")]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var model = new Models.News();
                var task = TryUpdateModelAsync(model);
                task.Wait();

                _repository.UpdateNews(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewsController/Delete/5
        [Authorize(Roles = "Admin, Owner")]
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetNewsByID(id);
            return View("Delete", model);
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Owner")]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteNews(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
