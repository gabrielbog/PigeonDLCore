using PigeonDLCore.Data;
using PigeonDLCore.Models;
using System.Security.Cryptography;
using System.Text;

namespace PigeonDLCore.Repository
{
    public class FolderRepository
    {
        private ApplicationDbContext dbContext;

        public FolderRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public FolderRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //operations
        public List<Folder> GetFoldersByIDUser(string IDUser)
        {
            List<Folder> folderList = new List<Folder>();

            foreach (Folder item in this.dbContext.Folders)
            {
                if (item.IDUser == IDUser)
                {
                    folderList.Add(item);
                }
            }

            return folderList;
        }
        public Folder GetFolderByIDFolder(Guid IDFolder)
        {
            return dbContext.Folders.FirstOrDefault(x => x.IDFolder == IDFolder);
        }

        public Folder GetFolderByURL(string URL)
        {
            return dbContext.Folders.FirstOrDefault(x => x.URL == URL);
        }

        public bool InsertFolder(Folder folder)
        {
            folder.IDFolder = Guid.NewGuid();
            folder.DateUploaded = DateTime.Now;

            Folder existingFolder = dbContext.Folders.FirstOrDefault(x => x.IDUser == folder.IDUser && x.Name == folder.Name);
            if (existingFolder != null)
                return false; //don't add folder if user already has folder with this name

            //generate url hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(folder.IDFolder.ToString() + folder.Name + folder.DateUploaded.ToString()));
                StringBuilder sBuilder = new StringBuilder();

                for(int i = 0; i < hashValue.Length; i++)
                {
                    sBuilder.Append(hashValue[i].ToString("x2"));
                }
                
                folder.URL = sBuilder.ToString();
            }

            dbContext.Folders.Add(folder);
            dbContext.SaveChanges();

            return true;
        }

        public bool UpdateFolder(Folder folder)
        {
            Folder existingfolder = dbContext.Folders.FirstOrDefault(x => x.IDFolder == folder.IDFolder);

            if (existingfolder != null)
            {
                Folder existingNameFolder = dbContext.Folders.FirstOrDefault(x => x.IDUser == folder.IDUser && x.Name == folder.Name);
                if (existingNameFolder != null)
                    return false; //don't edit folder if user already has folder with this name

                existingfolder.IDFolder = folder.IDFolder;
                existingfolder.IDUser = folder.IDUser;
                existingfolder.Name = folder.Name;
                existingfolder.DateUploaded = folder.DateUploaded;

                //remake existing URL hash, in case someone decides to upload a folder with same name then get the same result
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(folder.IDFolder.ToString() + folder.Name + folder.DateUploaded.ToString()));
                    StringBuilder sBuilder = new StringBuilder();

                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sBuilder.Append(hashValue[i].ToString("x2"));
                    }

                    existingfolder.URL = sBuilder.ToString();
                }
                dbContext.SaveChanges();
            }

            return true;
        }

        public void DeleteFolder(Guid IDFolder)
        {
            Folder existingfolder = dbContext.Folders.FirstOrDefault(x => x.IDFolder == IDFolder);

            if (existingfolder != null)
            {
                dbContext.Folders.Remove(existingfolder);
                dbContext.SaveChanges();
            }
        }
    }
}
