using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}