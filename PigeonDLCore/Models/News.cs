using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PigeonDLCore.Models
{
    public class News
    {
        [Key]
        public Guid IDNews { get; set; }

        [ForeignKey("Id")]
        public string IDUser { get; set; }
        public virtual IdentityUser Id { get; set; }

        //[Required]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        //[Required]
        [Column(TypeName = "varchar(200)")]
        public string Content { get; set; }

        //[Required]
        public DateTime DateAdded { get; set; }
    }
}
