using LMS_API.Data;
using LMS_API.Models.Publication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationController : Controller
    {
        private readonly ApplicationContext dbcontext;

        public PublicationController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }


        [HttpGet]
        [Route("GetAllPublications")]
        public async Task<IActionResult>  GetAllPublications()
        {
            var data = await dbcontext.Publications.ToListAsync();

            if (data != null)
            {
                return Ok(data);
            }

            else 
            {
                return NotFound();
            }
           
        }


        [HttpGet]
        [Route("GetPublicationById/{id}")]
        public async Task<IActionResult> GetPublicationById([FromRoute] Guid id)
        {
            var check = await dbcontext.Publications.FindAsync(id);

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
        [Route("AddPublication")]
        public async Task<IActionResult> AddPublication(AddPublicationRequest addPublicationRequest)
        {
            var checkIfPresent = await dbcontext.Publications.Where(p => p.PublicationName == addPublicationRequest.PublicationName).FirstOrDefaultAsync();

            if (checkIfPresent == null)
            {
                var data = new Publication()
                {
                    ID = Guid.NewGuid(),
                    PublicationName = addPublicationRequest.PublicationName,
                    PubAddress = addPublicationRequest.PubAddress,
                    PublicationPhNumber = addPublicationRequest.PublicationPhNumber,
                    Email = addPublicationRequest.Email

                };

                await dbcontext.Publications.AddAsync(data);
                await dbcontext.SaveChangesAsync();

                return Ok("Record added sucessfully!!");
            }

            else
            {
                return BadRequest(addPublicationRequest.PublicationName + " already exist in databae!! ");
            }
        }



        [HttpDelete]
        [Route("DeletePublication/{id}")]
        public async Task<IActionResult> DeletePublication([FromRoute] Guid id)
        {
            var check = await dbcontext.Publications.FindAsync(id);

            if (check != null)
            {
                dbcontext.Publications.Remove(check);
                await dbcontext.SaveChangesAsync();
                return Ok("Record Deleted Sucessfully!!");
            }

            else
            {
                return NotFound();
            }
        }



        [HttpPut]
        [Route("UpdatePublication/{id}")]
        public async Task<IActionResult> UpdatePublication([FromRoute] Guid id,UpdatePublicationRequest updatePublicationRequest)
        {
            try
            {
                var check = await dbcontext.Publications.FindAsync(id);

                if (check != null)
                {
                    check.PublicationName = updatePublicationRequest.PublicationName;
                    check.PubAddress = updatePublicationRequest.PubAddress;
                    check.PublicationPhNumber = updatePublicationRequest.PublicationPhNumber;
                    check.PubAddress = updatePublicationRequest.PubAddress;

                    await dbcontext.SaveChangesAsync();
                    return Ok("Record Updated Sucessfully!!");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the record.");
            }
           
            
        }
    }
}
