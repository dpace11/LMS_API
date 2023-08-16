using LMS_API.Data;
using LMS_API.Models.Author;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly ApplicationContext dbcontext;

        public AuthorController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var data = await dbcontext.Authors.ToListAsync();

            return Ok(data);
        }


        [HttpGet]
        [Route("GetAuthorById/{id}")]
        public async Task<IActionResult> GetAuthorById([FromRoute] Guid id)
        {

            var check = await dbcontext.Authors.FindAsync(id);
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
        [Route("AddAuthor")]
        public async Task<IActionResult> AddAuthor(AddAuthorRequest add)
        {
            var checkIfExist = await dbcontext.Authors.Where(a => a.AuthorUserName == add.AuthorUserName).FirstOrDefaultAsync();

            if (checkIfExist == null)
            {
                var data = new Author()
                {
                    ID = Guid.NewGuid(),
                    AuthorName = add.AuthorName,
                    AuthorUserName = add.AuthorUserName,
                    Address = add.Address,
                    PhoneNumber = add.PhoneNumber,
                    Email = add.Email,
                };

                await dbcontext.Authors.AddAsync(data);
                await dbcontext.SaveChangesAsync();
                return Ok("Data added successfully!!");
            }

            else
            {
                return BadRequest("Author with " + add.AuthorUserName + "alerady exist in database");
            }
        }



        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var check = await dbcontext.Authors.FindAsync(id);

            if (check != null)
            {
                dbcontext.Remove(check);
                await dbcontext.SaveChangesAsync();
                return Ok("Record Deleted Sucessfully!!");
            }
            else
            {
                return BadRequest("Record Not Found!!");
            }
        }


        [HttpPut]
        [Route("UpdateAuthor/{id}")]

        public async Task<IActionResult> UpdateAuthor([FromRoute] Guid id, UpdateAuthorRequest updateAuthorRequest)
        {
            var check = await dbcontext.Authors.FindAsync(id);

            if (check != null)
            {
                var data = new Author();
                data.Address = updateAuthorRequest.Address;
                data.PhoneNumber = updateAuthorRequest.PhoneNumber;
                data.Email = updateAuthorRequest.Email;

                await dbcontext.SaveChangesAsync();
                return Ok(data);
            }
            else
            {
                return BadRequest("Id not found to be updated");
            }
        }
    }
}