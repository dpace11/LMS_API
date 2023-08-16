using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.Author
{
    public class UpdateAuthorRequest
    {
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
