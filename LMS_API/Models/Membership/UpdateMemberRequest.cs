using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.Membership
{
    public class UpdateMemberRequest
    {
        

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Required]
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        public DateTime MembershipIssueDate { get; set; }


        
    }
}
