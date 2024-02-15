using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using PigeonDLCore.Data;
using PigeonDLCore.Models;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace PigeonDLCore.Controllers
{
    public class FileController : Controller
    {
        private Repository.FileRepository _fileRepository;
        private Repository.FolderRepository _folderRepository;

        public FileController(ApplicationDbContext dbContext)
        {
            _fileRepository = new Repository.FileRepository(dbContext);
            _folderRepository = new Repository.FolderRepository(dbContext);
        }

        // GET: FileController
        public ActionResult Index(string URL)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userID != null)
            {
                //user is logged in
                Folder existingFolder = _folderRepository.GetFolderByURL(URL);

                if (existingFolder != null)
                {
                    //said URL is valid
                    if (userID == existingFolder.IDUser)
                    {
                        //let user access files if they created the folder
                        var files = _fileRepository.GetFilesByIDFolder(existingFolder.IDFolder);
                        ViewData["Title"] = existingFolder.Name;
                        return View("Index", files);
                    }
                    else
                    {
                        //user doesn't have access - todo
                        return Unauthorized();
                    }
                }
                else
                {
                    //URL is incorrect
                    return NotFound();
                }
            }
            else
            {
                //user is guest
                return Unauthorized();
            }
        }

        // GET: FileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FileController/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: FileController/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
