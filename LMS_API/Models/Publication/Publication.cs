using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.Publication
{
    public class Publication
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Enter publication name")]
        [Display(Name = "Publication Name")]
        public string PublicationName { get; set; }

        [Required(ErrorMessage = "Enter publication address")]
        [Display(Name = "Address")]
        public string PubAddress { get; set; }

        [Required(ErrorMessage = "Enter publication phone nuumber")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "Phone number must be 9 to 10 digits.")]
        public long PublicationPhNumber { get; set; }


        [Required(ErrorMessage = "Enter publication email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
