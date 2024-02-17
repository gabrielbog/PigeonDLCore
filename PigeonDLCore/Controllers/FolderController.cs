using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PigeonDLCore.Data;
using System.Security.Claims;

namespace PigeonDLCore.Controllers
{
    public class FolderController : Controller
    {
        private Repository.FolderRepository _folderRepository;
        private Repository.FileRepository _fileRepository;

        public FolderController (ApplicationDbContext dbContext)
        {
            _folderRepository = new Repository.FolderRepository (dbContext);
            _fileRepository = new Repository.FileRepository (dbContext);
        }

        // GET: FolderController
        public ActionResult Index()
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userID != null)
            {
                //user is logged in
                var folders = _folderRepository.GetFoldersByIDUser(userID);
                return View("Index", folders);
            }
            else
            {
                //user is guest
                return Unauthorized();
            }
        }

        // GET: FolderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FolderController/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: FolderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine(ModelState.IsValid);
                    Models.Folder model = new Models.Folder();
                    model.IDUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var task = TryUpdateModelAsync(model);
                    task.Wait();

                    bool res = _folderRepository.InsertFolder(model);
                    if (res)
                        return RedirectToAction(nameof(Index));
                    else
                        return View("Create");
                }
                catch
                {
                    return View("Create");
                }
            }
            else
            {
                return View("Create");
            }
        }

        // GET: FolderController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _folderRepository.GetFolderByIDFolder(id);
            return View("Edit", model);
        }

        // POST: FolderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.Folder model = new Models.Folder();
                var task = TryUpdateModelAsync(model);
                task.Wait();

                bool res = _folderRepository.UpdateFolder(model);
                if (res)
                    return RedirectToAction(nameof(Index));
                else
                    return View("Edit");
            }
            catch
            {
                return View("Edit");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: FolderController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _folderRepository.GetFolderByIDFolder(id);
            return View("Delete", model);
        }

        // POST: FolderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                List<Models.File>_fileList = _fileRepository.GetFilesByIDFolder(id);
                if(_fileList.Count == 0)
                {
                    //folder is empty
                    _folderRepository.DeleteFolder(id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //folder still has files
                    return View("Delete");
                }
            }
            catch
            {
                return View("Delete");
            }
        }
    }
}
