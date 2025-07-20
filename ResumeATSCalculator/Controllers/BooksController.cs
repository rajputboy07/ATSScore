using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeATSCalculator.Models;

namespace ResumeATSCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {


        static private List<Book> books = new List<Book>
         {
             new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", YearPublished = 1925 },
             new Book { Id = 2, Title = "Pride and Prejudice", Author = "Jane Austen", YearPublished = 1813 },
             new Book { Id = 3, Title = "1984", Author = "George Orwell", YearPublished = 1949 },
             new Book { Id = 4, Title = "To Kill a Mockingbird", Author = "Harper Lee", YearPublished = 1960 },
             new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", YearPublished = 1951 }
         };

        [HttpGet()]
        public ActionResult<List<Book>> GetBooks()
        {
            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<List<Book>> GetBookById(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if (book != null)
                return Ok(book);
            else
                return NotFound();
        } 

        [HttpPost("add")]
        public ActionResult<List<Book>> AddBook(Book newbook)
        {
            books.Add(newbook);
            return CreatedAtAction(nameof(GetBookById),new {id = newbook.Id}, newbook);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.YearPublished = updatedBook.YearPublished;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if (book == null)
                return NotFound();

            books.Remove(book);
            return NoContent(); // 204 No Content – successful delete
        }



    }


}



