using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PigeonDLCore.Models
{
    [Table("Files")]
    public class File
    {
        [Key]
        public Guid IDFile { get; set; }

        [ForeignKey("Id")]
        public string IDUser { get; set; }
        public virtual IdentityUser Id { get; set; }

        public Guid IDFolder { get; set; }

        //[Required]
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

        //[Required]
        public DateTime DateUploaded { get; set; }

        //[Required]
        public double Size { get; set; }

        //[Required]
        [Column(TypeName = "varchar(32)")] //md5
        public string URL { get; set; }

        //[Required]
        public int Downloads { get; set; }

        [Column(TypeName = "varchar(256)")] //sha-256
        public string? Password { get; set; }

        [NotMapped]
        public IFormFile UploadedFile { get; set; } //the file itself

        //[ForeignKey("IDFolder")]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual Folder Folder { get; set; }
    }
}
