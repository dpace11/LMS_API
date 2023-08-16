using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.BookIssueReturn
{
    public class BookReturnRequest
    {
        [Display(Name = "Roll No")]
        [Range(750101, 750599, ErrorMessage = "Roll no shoud be within 7501XX--7505XX")]
        [Required]
        public int RollNo { get; set; }


        [Required]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Returned Date")]

        public DateTime BookReturnDate { get; set; }
    }
}
