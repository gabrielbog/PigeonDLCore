using PigeonDLCore.Data;
using PigeonDLCore.Models;
using System.Security.Cryptography;
using System.Text;
using File = PigeonDLCore.Models.File;

namespace PigeonDLCore.Repository
{
    public class FileRepository
    {
        private ApplicationDbContext dbContext;

        public FileRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public FileRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //operations
        public List<File> GetFilesByIDFolder(Guid IDFolder)
        {
            List<File> fileList = new List<File>();

            foreach (File item in this.dbContext.Files)
            {
                if (item.IDFolder == IDFolder)
                {
                    fileList.Add(item);
                }
            }

            return fileList;
        }

        public File GetFileByURL(string URL)
        {
            return dbContext.Files.FirstOrDefault(x => x.URL == URL);
        }

        public void InsertFile(File file)
        {
            Console.WriteLine(file.IDFolder.ToString());
            file.IDFile = Guid.NewGuid();
            file.DateUploaded = DateTime.Now;
            file.Downloads = 0;

            //generate url hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(file.IDFile.ToString() + file.Name + file.DateUploaded.ToString() + file.Size));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < hashValue.Length; i++)
                {
                    sBuilder.Append(hashValue[i].ToString("x2"));
                }

                file.URL = sBuilder.ToString();
            }

            dbContext.Files.Add(file);
            dbContext.SaveChanges();
        }

        public void UpdateFile(File file)
        {
            File existingFile = dbContext.Files.FirstOrDefault(x => x.IDFile == file.IDFile);

            if (existingFile != null)
            {
                existingFile.IDFile = file.IDFile;
                existingFile.IDUser = file.IDUser;
                existingFile.IDFolder = file.IDFolder;
                existingFile.Name = file.Name;
                existingFile.DateUploaded = file.DateUploaded;
                existingFile.Size = file.Size;
                existingFile.URL = file.URL;
                existingFile.Downloads = file.Downloads;
                dbContext.SaveChanges();
            }
        }

        public void DeleteFile(File file)
        {
            File existingFile = dbContext.Files.FirstOrDefault(x => x.IDFile == file.IDFile);

            if (existingFile != null)
            {
                dbContext.Files.Remove(existingFile);
                dbContext.SaveChanges();
            }
        }
    }
}
