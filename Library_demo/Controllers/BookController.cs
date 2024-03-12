using AutoMapper;
using Library_demo.Dtos;
using Library_demo.Interfaces;
using Library_demo.Models;
using Library_demo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBookRepository bookRepository, IUserRepository userRepository, IAuthorRepository authorRepository, IMapper mapper) : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        [ProducesResponseType(404)]
        public IActionResult GetBooks()
        {
            var books = mapper.Map<List<BookDto>>(bookRepository.GetBooks());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(books);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(404)]
        public IActionResult GetBook(int bookId)
        {

            if (!bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var book = mapper.Map<BookDto>(bookRepository.GetBook(bookId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateBook([FromQuery] int lenderId, [FromQuery] int authorId, [FromBody] BookDto bookObject)
        {

            if (bookObject == null)
            {
                return BadRequest();
            }

            if (bookRepository.BookExists(bookObject.Id))
            {
                ModelState.AddModelError("DuplicateError", "Book with that id already exists");
                return StatusCode(422, ModelState);
            }

            var mappedBook = mapper.Map<Book>(bookObject);

            if (!bookRepository.CreateBook(mappedBook, lenderId, authorId))
            {
                ModelState.AddModelError("SavingError", "Book could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("Book added successfully");
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateBook(int bookId, [FromBody] BookDto bookObject)
        {
            if (bookObject == null)
            {
                return BadRequest(ModelState);
            }

            if (bookObject.Id != bookId)
            {
                return BadRequest(ModelState);
            }

            if (!bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var mappedBook = mapper.Map<Book>(bookObject);
            if (!bookRepository.UpdateBook(mappedBook))
            {
                ModelState.AddModelError("SavingError", "Book could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("Book updated successfully");
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBook(int bookId)
        {

            if (!bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var book = bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!bookRepository.DeleteBook(book))
            {
                ModelState.AddModelError("SavingError", "Book could not be deleted");
                return StatusCode(500, ModelState);
            }

            return Ok("Book deleted successfully");
        }

        [HttpPut("lendBook")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult LendBook([FromQuery] int userId, [FromQuery] int bookId)
        {
            if (!userRepository.UserExists(userId))
            {
                return NotFound();
            }

            if (!bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var book = bookRepository.GetBook(bookId);
            var user = userRepository.GetUser(userId);
            var lender = bookRepository.GetLender(bookId);

            if (lender != null)
            {
                ModelState.AddModelError("LendingError", "Book is already lent");
                return StatusCode(422, ModelState);
            }

            book.Lender = user;

            if (!bookRepository.UpdateBook(book))
            {
                ModelState.AddModelError("SavingError", "Book could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok($"{book.Title} was lent to {user.FirstName} {user.LastName}");
        }

        [HttpPut("returnBook")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult ReturnBook([FromQuery] int bookId)
        {
            if (!bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!bookRepository.ReturnBook(bookId))
            {
                ModelState.AddModelError("SavingError", "Book could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("Book was returned");
        }

        [HttpGet("author/{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBooksByAuthorName(string name)
        {
            var books = mapper.Map<List<BookDto>>(bookRepository.GetBooksByAuthorName(name));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(books.Count == 0)
            {
                return NotFound(ModelState);
            }

            return Ok(books);
        }
    }
}
