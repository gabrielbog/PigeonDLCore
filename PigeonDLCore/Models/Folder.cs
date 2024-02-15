using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PigeonDLCore.Models
{
    [Table("Folders")]
    public class Folder
    {
        [Key]
        public Guid IDFolder { get; set; }

        [ForeignKey("Id")]
        public string IDUser { get; set; }
        public virtual IdentityUser Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

        //[Required]
        public DateTime DateUploaded { get; set; }

        //[Required]
        [Column(TypeName = "varchar(32)")] //md5
        public string URL { get; set; }

        [Column(TypeName = "varchar(256)")] //sha-256
        public string? Password { get; set; }
    }
}
