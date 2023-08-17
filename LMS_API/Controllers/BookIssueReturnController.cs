using LMS_API.Data;
using LMS_API.Models.BookIssueReturn;
using LMS_API.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookIssueReturnController : Controller
    {
        private readonly ApplicationContext dbcontext;

        public BookIssueReturnController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }


        [HttpGet]
        [Route("GetAllBookIssueReturnDetails")]
        public async Task<IActionResult> GetAllBookIssueReturnDetails()
        {
            var data = await dbcontext.BookIssueReturns.ToListAsync();

            return Ok(data);
        }


        [HttpGet]
        [Route("GetHistoryByRollNo/{rollno}")]
        public async Task<IActionResult> GetHistoryByRollNo([FromRoute] int rollno)
        {
            var check = await dbcontext.BookIssueReturns.Where(b => b.RollNo == rollno).ToListAsync();

            if (check != null)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest("history of " + rollno + "not present");
            }
        }


        [HttpGet]
        [Route("GetBooksToSubmitByRoll/{rollno}")]
        public async Task<IActionResult> GetBooksToSubmitByRoll([FromRoute] int rollno)
        {
            var data = await dbcontext.BookIssueReturns.Where(b => b.RollNo == rollno && b.BookReturnDate == null).ToListAsync();

            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest("No books left to be submitted by " + rollno);
            }
        }


        [HttpGet]
        [Route("GetBookIssueReturnDetailById/{id}")]
        public async Task<IActionResult> GetBookIssueReturnDetailById([FromRoute] Guid id)
        {
            var check = await dbcontext.BookIssueReturns.FindAsync(id);

            if (check != null)
            {
                return Ok(check);
            }
            else
            {
                return NotFound();
            }
        }




        [HttpDelete]
        [Route("DeletBookIssueRetunDetail/{id}")]
        public async Task<IActionResult> DeleteBookIssueReturnDetail([FromRoute] Guid id)
        {
            var check = await dbcontext.BookIssueReturns.FindAsync(id);

            if (check != null)
            {
                dbcontext.BookIssueReturns.Remove(check);
                await dbcontext.SaveChangesAsync();
                return Ok("Record deleted Sucessfully!!");
            }

            else
            {
                return NotFound();
            }
        }



        [HttpPost]
        [Route("IssueBook")]
        public async Task<IActionResult> IssueBook(IssueBookRequest issueBook)
        {
            var isRollPresent = await dbcontext.Students.AnyAsync(s => s.StudentRollNo == issueBook.RollNo);

            var memberExist = await dbcontext.Memberships.Where(m => m.StudentRollNo == issueBook.RollNo).FirstOrDefaultAsync();

            var rollNameDepartmentMatch = await dbcontext.Students.Where(s => s.StudentRollNo == issueBook.RollNo && s.StudentName == issueBook.FullName && s.Department == issueBook.Department).FirstOrDefaultAsync();

            var remainingQty = await dbcontext.Books.Where(b => b.BookName == issueBook.BookName).Select(b => b.RemainingQuantity).FirstOrDefaultAsync();

            var studentBookCount = await dbcontext.BookIssueReturns.Where(b => b.RollNo == issueBook.RollNo && b.BookReturnDate == null).CountAsync();

            var duplicateBook = await dbcontext.BookIssueReturns.Where(b => b.RollNo == issueBook.RollNo && b.BookName == issueBook.BookName && b.BookReturnDate == null).FirstOrDefaultAsync();

            var membershipDeadline = await dbcontext.Memberships.Where(m => m.StudentRollNo == issueBook.RollNo).Select(m => m.MembershipEndDate).FirstOrDefaultAsync();

            var isBookPresent = await dbcontext.Books.AnyAsync(b => b.BookName == issueBook.BookName);

            if (isRollPresent)
            {
                if (memberExist != null)
                {
                    if (rollNameDepartmentMatch != null)
                    {
                        if (isBookPresent)
                        {
                            if (remainingQty > 1)
                            {
                                if (studentBookCount <= 4)
                                {
                                    if (duplicateBook == null)
                                    {
                                        var data = new BookIssueReturn()
                                        {
                                            Id = Guid.NewGuid(),
                                            RollNo = issueBook.RollNo,
                                            FullName = issueBook.FullName,
                                            Department = issueBook.Department,
                                            BookName = issueBook.BookName,
                                            BookIssueDate = issueBook.BookIssueDate,
                                            DeadlineDate = issueBook.BookIssueDate.AddDays(14)

                                        };

                                        var membershipDeadline_VS_BookDeadline = data.DeadlineDate > membershipDeadline;

                                        if (membershipDeadline_VS_BookDeadline != true)
                                        {
                                            var bookname = dbcontext.Books.Where(b => b.BookName ==issueBook.BookName).FirstOrDefault();

                                            bookname.RemainingQuantity--;

                                            await dbcontext.BookIssueReturns.AddAsync(data);
                                            await dbcontext.SaveChangesAsync();

                                            return Ok("Book issued to " + issueBook.RollNo);
                                        }
                                        else
                                        {
                                            return BadRequest("The membership deadline exceed the book deadline date. Renew membership of " + issueBook.RollNo + "to issue book");
                                        }

                                    }
                                    else
                                    {
                                        return BadRequest(issueBook.BookName + " is already issued to " + issueBook.RollNo);
                                    }

                                }
                                else
                                {
                                    return BadRequest("Exceeded the limit of 5 books per student");
                                }


                            }
                            else
                            {
                                return BadRequest("Book out of stock. Only available for reading in library!!");
                            }
                        }
                        else
                        {
                            return BadRequest("Book is not in library. Enter book details first!!");
                        }


                    }
                    else
                    {
                        return BadRequest("Rollnumber, Name,Deparment mismatch occured");
                    }

                }
                else
                {
                    return BadRequest(issueBook.RollNo + "is not a member of library. Issue membership first");
                }
            }
            else
            {
                return BadRequest("Roll number not in database");
            }


        }
        


        [HttpPut]
        [Route("ReturnBook /{id})")]
        public async Task<IActionResult> ReturnBook([FromRoute] Guid id, BookReturnRequest bookReturnRequest)
        {
            var check = await dbcontext.BookIssueReturns.FindAsync(id);

            var bookname = dbcontext.Books.Where(b => b.BookName == bookReturnRequest.BookName).FirstOrDefault();

            var checkIfBookIssuedOrNot = await dbcontext.BookIssueReturns.
                Where(b => b.RollNo == bookReturnRequest.RollNo && b.BookName == bookReturnRequest.BookName && b.BookReturnDate == null).FirstOrDefaultAsync();


            if (check != null)
            {
                int rollno = await dbcontext.BookIssueReturns.Where(b => b.Id ==id).Select(x=>x.RollNo).FirstOrDefaultAsync();

                if (rollno == bookReturnRequest.RollNo)
                {
                    if (checkIfBookIssuedOrNot != null)
                    {
                        check.BookReturnDate = bookReturnRequest.BookReturnDate;

                        bookname.RemainingQuantity++;

                        await dbcontext.SaveChangesAsync();
                        return Ok(check);
                    }
                    else
                    {
                        return BadRequest("This book has not been issued to " + bookReturnRequest.RollNo);
                    }
                  
                }
                else
                {
                    return BadRequest("id and rollno doesnot match");
                }
            }
            else
            {
                return NotFound();
            }


        }



       /* [HttpPut]
        [Route("BookDeadlineExtend/{rollno}")]
        public async Task<IActionResult> BookDeadlineExtend([FromRoute] int rollno, BookDeadlineExtendRequest deadlineExtendRequest)
        {
            var check = await dbcontext.BookIssueReturns.Where(b => b.RollNo == rollno && b.BookReturnDate == null).FirstOrDefaultAsync();

            var oldDeadlineDate= dbcontext.BookIssueReturns.Where(b=>b.RollNo==rollno && b.BookName==deadlineExtendRequest.BookName && b.BookReturnDate==null).Select(b=>b.DeadlineDate).FirstOrDefaultAsync();

            var oldIssueDate= dbcontext.BookIssueReturns.Where(b => b.RollNo == rollno && b.BookName == deadlineExtendRequest.BookName && b.BookReturnDate == null).Select(b => b.BookIssueDate).FirstOrDefaultAsync();

            var newIssueDate = deadlineExtendRequest.BookIssueDate;

            var datediff = int(deadlineExtendRequest.BookIssueDate - oldDeadlineDate).totaldays;
            TimeSpan datediff= deadlineExtendRequest.BookIssueDate - oldDeadlineDate


            if (check != null)
            {
                
            }
            else
            {
                return BadRequest("cannot extend deadline!! no any book to be returned");
            }
        }
*/
    }
}
