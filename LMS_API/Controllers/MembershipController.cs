using LMS_API.Data;
using LMS_API.Models.Membership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : Controller
    {
        private readonly ApplicationContext dbcontext;

        public MembershipController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }


        [HttpGet]
        [Route("GetAllMembers")]
        public async Task<IActionResult> GetAllMembers()
        {
            var data = await dbcontext.Memberships.OrderBy(x => x.StudentRollNo).ToListAsync();

            return Ok(data);
        }


        [HttpGet]
        [Route("GetMemberById/{id}")]
        public async Task<IActionResult> GetMemberById([FromRoute] Guid id)
        {
            var check = await dbcontext.Memberships.FindAsync(id);

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
        [Route("AddMembers")]
        public async Task<IActionResult> AddMembers(AddMembersRequest addMembersRequest)
        {
            bool roll = dbcontext.Students.Any(c => c.StudentRollNo == addMembersRequest.StudentRollNo);

            bool member_already_exist = dbcontext.Memberships.Any(m => m.StudentRollNo == addMembersRequest.StudentRollNo);

            var roll_name_match = await dbcontext.Students.Where(s => s.StudentRollNo == addMembersRequest.StudentRollNo && s.StudentName == addMembersRequest.FullName).FirstOrDefaultAsync();

            if (member_already_exist != true)
            {

                if (roll)
                {
                    if (roll_name_match != null)
                    {
                        var data = new Membership()
                        {
                            MembershipId = Guid.NewGuid(),
                            StudentRollNo = addMembersRequest.StudentRollNo,
                            FullName = addMembersRequest.FullName,
                            MembershipIssueDate = addMembersRequest.MembershipIssueDate,
                            MembershipEndDate = addMembersRequest.MembershipIssueDate.AddMonths(3)
                        };
                        await dbcontext.Memberships.AddAsync(data);
                        await dbcontext.SaveChangesAsync();
                        return Ok("Record added sucessfully!!");
                    }
                    else
                    {
                        return BadRequest("Rollno Name mis-match. Enter correct detail.");
                    }
                }
                else
                {
                    return BadRequest(addMembersRequest.StudentRollNo + " not in database. Enter into database first.");
                }
            }
            else
            {
                return BadRequest(addMembersRequest.StudentRollNo + " is already a member of library.");
            }
        }



        [HttpDelete]
        [Route("DeleteMember/{id}")]
        public async Task<IActionResult> DeleteMember([FromRoute] Guid id)
        {
            var check = await dbcontext.Memberships.FindAsync(id);

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


        [HttpPut]
        [Route("UpdateMembers/{id}")]
        public async Task<IActionResult> UpdateMembers([FromRoute] Guid id, UpdateMemberRequest updateMemberRequest)
        {
            var data = await dbcontext.Memberships.FindAsync(id);



            if (data != null)
            {
                data.FullName = updateMemberRequest.FullName;
                data.MembershipIssueDate = updateMemberRequest.MembershipIssueDate;
                data.MembershipEndDate = updateMemberRequest.MembershipIssueDate.AddMonths(3);

                await dbcontext.SaveChangesAsync();
                return Ok( data);
            }
            else
            {
                return NotFound( "Did not match id");
            }
        }
    }
}
