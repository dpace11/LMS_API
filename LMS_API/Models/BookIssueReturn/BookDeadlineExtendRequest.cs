using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.BookIssueReturn
{
    public class BookDeadlineExtendRequest
    {
        [Display(Name = "Roll No")]
        [Range(750101, 750599, ErrorMessage = "Roll no shoud be within 7501XX--7505XX")]
        [Required]
        public int RollNo { get; set; }

        [Required]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Issued Date")]
        public DateTime BookIssueDate { get; set; }
    }
}
