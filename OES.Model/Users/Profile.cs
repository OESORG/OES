using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Users
{
    public class Profile : BaseEntity
    {
        public Profile()
        {
            ProfileId = GenerateKey();
        }

        [Key]
        public string ProfileId { get; set; }

        [StringLength(1024)]
        public string Avatar { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
