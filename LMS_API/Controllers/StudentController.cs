using LMS_API.Data;
using LMS_API.Models.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly ApplicationContext dbcontext;

        public StudentController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("GetAllStudent")]
        public async Task<IActionResult> GetStudent()
        {
            var data = await dbcontext.Students.OrderBy(x=>x.StudentRollNo).ToListAsync();

            return Ok(data);
        }

        [HttpGet]
        [Route("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] Guid id)
        {
            var check = await dbcontext.Students.FindAsync(id);

            if (check != null)
            {
                return Ok(check);
            }
            else
            {
                return NotFound(); 
            }
        }


        [HttpPost]
        [Route("AddStudent")]
        public async Task<IActionResult> AddStudent(AddStudentRequest addStudentRequest)
        {
            var rollAlreadyPresent =await dbcontext.Students.Where(s => s.StudentRollNo == addStudentRequest.StudentRollNo).FirstOrDefaultAsync();

            if (rollAlreadyPresent == null)
            {
                var data = new Students()
                {
                    StudentID = Guid.NewGuid(),
                    StudentRollNo = addStudentRequest.StudentRollNo,
                    StudentName = addStudentRequest.StudentName,
                    Department = addStudentRequest.Department,
                    Semester = addStudentRequest.Semester,
                    StudentContact = addStudentRequest.StudentContact,
                    StudentEmail = addStudentRequest.StudentEmail
                };
                await dbcontext.Students.AddAsync(data);
                await dbcontext.SaveChangesAsync();
                return Ok("Record added sucessfully!!");
            }
            else
            {
                return BadRequest("Roll number already exist!!");
            }
        }

        [HttpPut]
        [Route("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, UpdateStudentRequest updateStudentRequest)
        {
            var check = await dbcontext.Students.FindAsync(id);
            var check_if_already_exist = await dbcontext.Students.Where(x => x.StudentRollNo == updateStudentRequest.StudentRollNo).FirstOrDefaultAsync();
            if (check != null)
            {
                if (check_if_already_exist == null)
                {
                    check.StudentRollNo = updateStudentRequest.StudentRollNo;
                    check.StudentName = updateStudentRequest.StudentName;
                    check.Department = updateStudentRequest.Department;
                    check.Semester = updateStudentRequest.Semester;
                    check.StudentContact = updateStudentRequest.StudentContact;
                    check.StudentEmail = updateStudentRequest.StudentEmail;

                    await dbcontext.SaveChangesAsync();
                    return Ok("Record Updated Sucessfully!!");
                }
                else
                {
                    return BadRequest("Roll number already exist!!");
                }

            }

            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            var check = await dbcontext.Students.FindAsync(id);

            if (check != null)
            {
                dbcontext.Remove(check);
                await dbcontext.SaveChangesAsync();

                return Ok("Record has been sucessfully deleted!!");

            }

            else
            { 
                return NotFound();
            }

        }
    }
}
