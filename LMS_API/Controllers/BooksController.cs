﻿using LMS_API.Data;
using LMS_API.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly ApplicationContext dbcontext;

        public BooksController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }


        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var data = await dbcontext.Books.ToListAsync();
            return Ok(data);
        }



        [HttpGet]
        [Route("GetBookById/{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] Guid id)
        {
            var books = await dbcontext.Books.FindAsync(id);

            if (books != null)

            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }

        }

       

        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBook(AddBookRequest addbookRequest)
        {          

            var isBookAlreadyPresent = await dbcontext.Books.AnyAsync(b => b.BookName == addbookRequest.BookName);

            var isAuthorInDb = await dbcontext.Authors.AnyAsync(a => a.AuthorName == addbookRequest.AuthorName);

            var isPublicationInDb = await dbcontext.Publications.AnyAsync(p => p.PublicationName == addbookRequest.PublicationName);
            

            if (!isBookAlreadyPresent)
            {
                if (isAuthorInDb)
                {
                    if (isPublicationInDb)
                    {
                        var book = new Books()
                        {
                            BookId = Guid.NewGuid(),
                            BookName = addbookRequest.BookName,
                            AuthorName = addbookRequest.AuthorName,
                            PublicationName = addbookRequest.PublicationName,
                            Price = addbookRequest.Price,
                            PurchaseDate = addbookRequest.PurchaseDate,
                            Quantity = addbookRequest.Quantity,
                            BookLocation = addbookRequest.BookLocation,
                            RemainingQuantity = addbookRequest.RemainingQuantity
                        };

                        await dbcontext.Books.AddAsync(book);
                        await dbcontext.SaveChangesAsync();

                        return Ok(book);


                    }
                    else
                    {
                        return BadRequest("Publication details is not in Db. Enter publication details first!!");
                    }
                }
                else
                {
                    return BadRequest("Author details is not in Db. Enter author details first!!");
                }
                
            }
            else
            {
                return BadRequest("Book name already exist!!");
            }

        }



        [HttpPut]
        [Route("UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, UpdateBookRequest updateBookRequest)
        {
            var books = await dbcontext.Books.FindAsync(id);

            var isAuthorInDb = await dbcontext.Authors.AnyAsync(a => a.AuthorName == updateBookRequest.AuthorName);

            var isPublicationInDb = await dbcontext.Publications.AnyAsync(p => p.PublicationName == updateBookRequest.PublicationName);


            if (books != null)
            {

                if (isAuthorInDb)
                {
                    if (isPublicationInDb)
                    {
                        books.AuthorName = updateBookRequest.AuthorName;
                        books.PublicationName = updateBookRequest.PublicationName;
                        books.Price = updateBookRequest.Price;
                        books.PurchaseDate = updateBookRequest.PurchaseDate;
                        books.Quantity = updateBookRequest.Quantity;
                        books.BookLocation = updateBookRequest.BookLocation;
                        books.RemainingQuantity = updateBookRequest.RemainingQuantity;

                        await dbcontext.SaveChangesAsync();
                        return Ok(books);
                    }
                    else
                    {
                        return BadRequest("The book's publication you are trying to update is not in DB. You must first enter publication's detail in DB ");

                    }

                }
                else
                {
                    return BadRequest("The book's author you are trying to update is not in DB. You must first enter author's detail in DB ");
                }


                
               
            }
            else
            {

                return NotFound();
            }


        }
           


        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var books = await dbcontext.Books.FindAsync(id);

            if (books != null)
            {
                 dbcontext.Remove(books);
                await dbcontext.SaveChangesAsync();

                return Ok(books);
            }
            return NotFound();
        }
    }
}
