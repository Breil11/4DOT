using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.SQLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApiRestContext _context;

        public BooksController(ApiRestContext context)
        {
            _context = context;
        }

        


        //Add a book
        [HttpPost]
        public IActionResult AddBook(Books book)
        {
            _context.Bookss.Add(book);
            _context.SaveChanges();
            return Ok();
        }

        //Modify an existing book
        [HttpPut("{title}")]
        public IActionResult ModifyBooks(string title, string? newTitle)
        {
            Books book = _context.Bookss.FirstOrDefault(l => l.Title == title);
            if (book != null)
            {
                book.Title = newTitle;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        //Delete an existing book
       [HttpDelete("ByTitle/{title}")]
        public IActionResult DeleteBooks(string? title)
        {
            Books book = _context.Bookss.FirstOrDefault(l => l.Title == title);
            if (book != null)
            {
                _context.Bookss.Remove(book);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        //Select a book by its title
        [HttpGet("{title}")]
        public IActionResult SelectBookByTitle(string title)
        {
            Books book = _context.Bookss.FirstOrDefault(l => l.Title == title);
            if (book != null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound();
            }
        }

        //Select all books who contains a given word
        [HttpGet("contains/{word}")]
        public IActionResult SelectBooksWithWord(string word)
        {
            IEnumerable<Books> books = _context.Bookss.Where(l => l.Title.Contains(word));
            if (books.Any())
            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }
        }


        //Select all existing books in the database
        //GET: api/Books
        [HttpGet("GetBooks")]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
        {
            return await _context.Bookss.ToListAsync();
        }


        //Select all the books of the same author
        [HttpGet("auteur/{AuthorName}")]
        public IActionResult SelectBooksByAuthor(string AuthorName)
        {
            IEnumerable<Books> books = _context.Bookss.Where(l => l.NameAuthor == AuthorName);
            if (books.Any())
            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }
        }

        //Bonus1
        //change price of books by title
        [HttpPut("bonus1: change-price/{title}")]
        public IActionResult ChangeBookPrice(string title, int newPrice)
        {
            Books book = _context.Bookss.FirstOrDefault(l => l.Title == title);
            if (book != null)
            {
                book.Price = newPrice;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        //Bonus2
        //select books by type
        [HttpGet("bonus2: genre/{followedtype}")]
        public IActionResult GetBooksByGenre(string followedtype)
        {
            IEnumerable<Books> books = _context.Bookss.Where(l => l.Type == followedtype);
            if (books.Any())
            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }
        }

        //Bonus3
        //select book wich has price less than a value
        [HttpGet("bonus3: price-less-than/{value}")]
        public IActionResult GetBooksPriceLessThan(decimal value)
        {
            IEnumerable<Books> books = _context.Bookss.Where(l => l.Price < value);
            if (books.Any())
            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }
        }



    }
}
