using AutoMapper;
using Library_demo.Dtos;
using Library_demo.Interfaces;
using Library_demo.Models;
using Library_demo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Library_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController (IAuthorRepository authorRepository, IMapper mapper) : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        [ProducesResponseType(404)]
        public IActionResult GetAuthors()
        {
            var authors = mapper.Map<List<AuthorDto>>(authorRepository.GetAuthors());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(404)]
        public IActionResult GetAuthor(int authorId)
        {

            if (!authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var author = mapper.Map<Author>(authorRepository.GetAuthor(authorId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(author);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateAuthor([FromQuery] int bookId, [FromBody] AuthorDto authorObject)
        {

            if (authorObject == null)
            {
                return BadRequest();
            }

            if (authorRepository.AuthorExists(authorObject.Id))
            {
                ModelState.AddModelError("DuplicateError", "Author with that id already exists");
                return StatusCode(422, ModelState);
            }

            var mappedAuthor = mapper.Map<Author>(authorObject);

            if (!authorRepository.CreateAuthor(mappedAuthor, bookId))
            {
                ModelState.AddModelError("SavingError", "Author could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("Author added successfully");
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateAuthor(int authorId, [FromBody] AuthorDto authorObject)
        {
            if (authorObject == null)
            {
                return BadRequest(ModelState);
            }

            if (authorObject.Id != authorId)
            {
                return BadRequest(ModelState);
            }

            if (!authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var mappedAuthor = mapper.Map<Author>(authorObject);
            if (!authorRepository.UpdateAuthor(mappedAuthor))
            {
                ModelState.AddModelError("SavingError", "Author could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("Author updated successfully");
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            
            var author = authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!authorRepository.DeleteAuthor(author))
            {
                ModelState.AddModelError("SavingError", "Author could not be deleted");
                return StatusCode(500, ModelState);
            }

            return Ok("Author deleted successfully");
        }

        [HttpGet("book/{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthorsByBookTitle(string name)
        {
            var authors = mapper.Map<List<AuthorDto>>(authorRepository.GetAuthorsByBookTitle(name));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (authors.Count == 0)
            {
                return NotFound(ModelState);
            }

            return Ok(authors);
        }
    }
}
