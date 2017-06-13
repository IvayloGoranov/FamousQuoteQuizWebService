using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Models
{
    public class UserSession : BaseModel<int>
    {
        [Required]
        [MaxLength(1024)]
        public string AuthToken { get; set; }

        [Required]
        public DateTime ExpirationDateTime { get; set; }

        [Required]
        [ForeignKey("OwnerUser")]
        public string OwnerUserId { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }
    }
}
