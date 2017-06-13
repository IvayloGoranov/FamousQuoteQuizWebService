using System.ComponentModel.DataAnnotations;

namespace FamousQuoteQuiz.Server.Api.BindingModels.Account
{
    public class LoginUserBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}