using Microsoft.AspNetCore.Identity;
using PigeonDLCore.Data;
using PigeonDLCore.Models;
using System.Security.Cryptography;
using System.Text;

namespace PigeonDLCore.Repository
{
    public class FolderSharedRepository
    {
        private ApplicationDbContext dbContext;

        public FolderSharedRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public FolderSharedRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //operations
        public List<FolderShared> GetFolderSharedUsernamesByIDFolder(Guid IDFolder)
        {
            //this is going to need some deep thinking
            List<FolderShared> folderSharedList = new List<FolderShared>();

            foreach (FolderShared item in this.dbContext.FoldersShared)
            {
                if (item.IDFolder == IDFolder)
                {
                    folderSharedList.Add(item);
                }
            }

            return folderSharedList;
        }

        public bool CanIDUserAccesIDFolder(string IDUser, Guid IDFolder)
        {
            FolderShared folderShared = dbContext.FoldersShared.FirstOrDefault(x => x.IDUser == IDUser && x.IDFolder == IDFolder);
            if (folderShared == null)
                return false;
            return true;
        }

        public void InsertFolderShared(FolderShared folderShared)
        {
            folderShared.IDShared = Guid.NewGuid();

            dbContext.FoldersShared.Add(folderShared);
            dbContext.SaveChanges();
        }

        public void UpdatefolderShared(FolderShared folderShared)
        {
            FolderShared existingfolderShared = dbContext.FoldersShared.FirstOrDefault(x => x.IDShared == folderShared.IDShared);

            if (existingfolderShared != null)
            {
                existingfolderShared.IDShared = folderShared.IDShared;
                existingfolderShared.IDUser = folderShared.IDUser;
                existingfolderShared.IDFolder = folderShared.IDFolder;
                existingfolderShared.DateAdded = folderShared.DateAdded;
                dbContext.SaveChanges();
            }
        }

        public void DeletefolderShared(FolderShared folderShared)
        {
            FolderShared existingfolderShared = dbContext.FoldersShared.FirstOrDefault(x => x.IDShared == folderShared.IDShared);

            if (existingfolderShared != null)
            {
                dbContext.FoldersShared.Remove(existingfolderShared);
                dbContext.SaveChanges();
            }
        }
    }
}
