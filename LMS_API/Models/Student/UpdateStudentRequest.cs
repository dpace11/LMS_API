using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.Student
{
    public class UpdateStudentRequest
    {

        [Display(Name = "Roll No")]
        [Range(750101, 750599, ErrorMessage = "Roll no shoud be within 7501XX--7505XX")]
        [Required]
        public int StudentRollNo { get; set; }


        [Display(Name = "Student Name")]
        [Required]
        public string StudentName { get; set; }



        [Display(Name = "Department")]
        [Required]
        public string Department { get; set; }



        [Range(1, 8, ErrorMessage = "Semester should be between 1-8")]
        public int Semester { get; set; }



        [Display(Name = "Student Contact")]
        //[StringLength(10, ErrorMessage = "Student contact should not exceed 10 characters")]
        [Required]
        public long StudentContact { get; set; }


        [Display(Name = "Student Email")]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "A valid email required")]
        public string StudentEmail { get; set; }
    }
}
