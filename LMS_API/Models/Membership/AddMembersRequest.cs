using System.ComponentModel.DataAnnotations;
namespace LMS_API.Models.Membership
{
    public class AddMembersRequest
    {
        [Display(Name = "Roll No")]
        [Range(750101, 750599, ErrorMessage = "Roll no shoud be within 7501XX--7505XX")]
        [Required]
        public int StudentRollNo { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Required]
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        public DateTime MembershipIssueDate { get; set; }


   
    }
}
