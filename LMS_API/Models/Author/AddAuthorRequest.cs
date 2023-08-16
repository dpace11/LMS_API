using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.Author
{
    public class AddAuthorRequest
    {
        [Required(ErrorMessage = "Enter author name")]
        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }


        [Required(ErrorMessage = "Author username is a required field")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be atleat 4 character long")]
        public string AuthorUserName { get; set; }

        [Required(ErrorMessage = "Enter author's address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter author's phone number")]
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }


        [Required(ErrorMessage = "Enter author's email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
