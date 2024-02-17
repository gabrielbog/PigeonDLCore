using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PigeonDLCore.Models
{
    [Table("FoldersShared")]
    public class FolderShared
    {
        [Key]
        public Guid IDShared { get; set; }

        [ForeignKey("Id")]
        public string IDUser { get; set; }
        public virtual IdentityUser Id { get; set; }

        public Guid IDFolder { get; set; } //constraint issue despite IDFolder existing, removing FK for now

        //[Required]
        public DateTime DateAdded { get; set; }

        //[ForeignKey("IDFolder")]
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        //public virtual Folder Folder { get; set; }
    }
}
