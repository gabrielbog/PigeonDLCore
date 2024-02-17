using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PigeonDLCore.Data;
using PigeonDLCore.Models;
using System.Security.Claims;

namespace PigeonDLCore.Controllers
{
    public class FileController : Controller
    {
        private Repository.FileRepository _fileRepository;
        private Repository.FolderRepository _folderRepository;
        private Repository.FolderSharedRepository _folderSharedRepository;

        public FileController(ApplicationDbContext dbContext)
        {
            _fileRepository = new Repository.FileRepository(dbContext);
            _folderRepository = new Repository.FolderRepository(dbContext);
            _folderSharedRepository = new Repository.FolderSharedRepository(dbContext);
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
                        ViewData["AllowUploadDelete"] = "true";
                        ViewData["URL"] = URL;
                        return View("Index", files);
                    }
                    else
                    {
                        //check if user allowed other users to access folder
                        bool canAccess = _folderSharedRepository.CanIDUserAccesIDFolder(userID, existingFolder.IDFolder);
                        if(canAccess)
                        {
                            var files = _fileRepository.GetFilesByIDFolder(existingFolder.IDFolder);
                            ViewData["Title"] = existingFolder.Name;
                            ViewData["AllowUploadDelete"] = "false";
                            return View("Index", files);
                        }
                        else
                        {
                            return Unauthorized();
                        }
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

        // GET: FileController/Download
        public ActionResult Download(string URL)
        {
            var existingFile = _fileRepository.GetFileByURL(URL);
            if (existingFile != null)
            {
                //everyone can download the file if they have the correct URL
                ViewData["URL"] = URL;
                ViewData["FileName"] = existingFile.Name;
                return View("Download", existingFile);
            }
            else
            {
                //URL is incorrect
                return NotFound();
            }
        }

        // POST: FileController/Download
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Download(string URL, IFormCollection collection)
        {
            try
            {
                var existingFile = _fileRepository.GetFileByURL(URL);
                if (existingFile != null)
                {
                    //everyone can download the file if they have the correct URL
                    string rootFolder = @".\wwwroot\files";
                    string userID = existingFile.IDUser;
                    string folderID = existingFile.IDFolder.ToString();
                    string fileName = existingFile.Name;

                    string folderPath = rootFolder + @"\" + userID + @"\" + folderID;
                    string filePath = folderPath + @"\" + fileName;

                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                        _fileRepository.IncrementDownloadCounterByURL(URL);
                        return File(bytes, "application/octet-stream", fileName);
                    }
                    else
                    {
                        //file does not exist
                        return NotFound();
                    }
                }
                else
                {
                    //URL is incorrect
                    return NotFound();
                }
            }
            catch
            {
                ViewData["URL"] = URL;
                return View("Download");
            }
        }

        // GET: FileController/Upload
        public ActionResult Upload(string URL)
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
                        //folder creator can only view this
                        ViewData["URL"] = URL;
                        return View();
                    }
                    else
                    {
                        //only the user that created the folder should be able to upload
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

        // POST: FileController/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string URL, IFormCollection collection)
        {
            try
            {
                string rootFolder = @".\wwwroot\files";
                string userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userID != null)
                {
                    //user is logged in
                    Folder existingFolder = _folderRepository.GetFolderByURL(URL);

                    if (existingFolder != null)
                    {
                        //said URL is valid
                        if (userID == existingFolder.IDUser)
                        {
                            //folder creator can only create files
                            string folderID = existingFolder.IDFolder.ToString();
                            var files = collection.Files;

                            if (files.Count() > 0)
                            {
                                foreach (var file in files) //should be only one file
                                {
                                    string folderPath = rootFolder + @"\" + userID + @"\" + folderID;
                                    string filePath = folderPath + @"\" + file.FileName; //write file to this user's specific folder
                                    long fileSizeBytes = file.Length;

                                    Console.WriteLine(filePath);
                                    if(!System.IO.File.Exists(filePath))
                                    {
                                        Directory.CreateDirectory(folderPath);
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file.CopyTo(stream);
                                        }

                                        Models.File dbFile = new Models.File();
                                        dbFile.IDUser = userID;
                                        dbFile.IDFolder = existingFolder.IDFolder;
                                        dbFile.Name = file.FileName;
                                        dbFile.Size = fileSizeBytes;
                                        _fileRepository.InsertFile(dbFile);
                                        return RedirectToAction("Index", new { URL = URL });
                                    }
                                    else
                                    {
                                        ViewData["URL"] = URL;
                                        return View("Upload");
                                    }
                                }
                                return RedirectToAction("Index", new { URL = URL });
                            }
                            else
                            {
                                //no files
                                ViewData["URL"] = URL;
                                return View("Upload");
                            }
                        }
                        else
                        {
                            //don't allow anyone else
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        //URL invalid
                        return NotFound();
                    }
                }
                else
                {
                    //user is guest
                    return Unauthorized();
                }
            }
            catch
            {
                return View("Upload");
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(string URL)
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");

            if (userID != null)
            {
                //user is logged in
                var existingFile = _fileRepository.GetFileByURL(URL);

                if (existingFile != null)
                {
                    Console.WriteLine("Before Submit: " + URL);
                    bool showPage = false;

                    //said URL is valid
                    if (userID == existingFile.IDUser)
                    {
                        showPage = true;

                    }
                    else if(isAdmin == true)
                    {
                        showPage = true;
                    }
                    
                    if(showPage == true)
                    {
                        //folder creator or site admin can only view this
                        ViewData["URL"] = URL; //i can't delete the file otherwise
                        return View("Delete", existingFile);
                    }
                    else
                    {
                        //noone else should be able to
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

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string URL, IFormCollection collection)
        {
            Console.WriteLine("URL: " + URL);
            try
            {
                string rootFolder = @".\wwwroot\files";
                string userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");

                if (userID != null)
                {
                    //user is logged in
                    Models.File existingFile = _fileRepository.GetFileByURL(URL);

                    if (existingFile != null)
                    {
                        bool deletePerm = false;
                        Models.Folder existingFolder = _folderRepository.GetFolderByIDFolder(existingFile.IDFolder);

                        //said URL is valid
                        if (userID == existingFile.IDUser)
                        {
                            deletePerm = true;

                        }
                        else if (isAdmin == true)
                        {
                            deletePerm = true;
                        }

                        if (deletePerm == true)
                        {
                            //folder creator or site admin can delete files
                            string fileUserID = existingFile.IDUser.ToString();
                            string folderID = existingFile.IDFolder.ToString();

                            string folderPath = rootFolder + @"\" + fileUserID + @"\" + folderID;
                            string filePath = folderPath + @"\" + existingFile.Name;

                            if(System.IO.File.Exists(filePath))
                            {
                                //file exists
                                System.IO.File.Delete(filePath);
                                _fileRepository.DeleteFile(existingFile);

                                if(isAdmin == true)
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    if (existingFolder != null)
                                    {
                                        return RedirectToAction("Index", new { URL = existingFolder.URL });
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }
                            else
                            {
                                //file does not exist
                                ViewData["URL"] = URL; //i can't delete the file otherwise
                                return View("Delete");
                            }
                        }
                        else
                        {
                            //noone else should be able to
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
            catch
            {
                ViewData["URL"] = URL; //i can't delete the file otherwise
                return View("Delete");
            }
        }
    }
}
