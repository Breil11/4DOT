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
    public class BorrowersController : ControllerBase
    {
        private readonly ApiRestContext _context;
        private object b;

        public BorrowersController(ApiRestContext context)
        {
            _context = context;
        }

        //Add a borrower
        [HttpPost]
        public IActionResult AddBorrower(Borrowers borrower)
        {
            _context.Borrowerss.Add(borrower);
            _context.SaveChanges();
            return Ok();

        }

        //Modify a borrower
        [HttpPut("{firstname}/{lastname}")]
        public IActionResult ModifyBorrowers(string firstname, string? newFirstname, string lastname, string? newLastname)
        {
            Borrowers borrower = _context.Borrowerss.FirstOrDefault(a => a.Firstname == firstname);
            Borrowers borrower2 = _context.Borrowerss.FirstOrDefault(b => b.Lastname == lastname);

            if (borrower != null && borrower2 != null)
            {
                borrower.Firstname = newFirstname;
                borrower2.Lastname = newLastname;

                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        //Delete a borrower
        [HttpDelete("{firstname}/{lastname}")]
        public IActionResult DeleteBorrower(string firstname, string lastname)
        {
            // Recherche de l'emprunteur à supprimer
            Borrowers borrower = _context.Borrowerss.FirstOrDefault(b => b.Firstname == firstname && b.Lastname == lastname);

            if (borrower != null)
            {
                _context.Borrowerss.Remove(borrower);
                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        //Select a borrower by last name, first name or address
        [HttpGet("{firstname}/{lastname}/{Address}")]
        public IActionResult GetBorrower(string searchQuery)
        {
            var borrowers = _context.Borrowerss
                .Where(b => b.Firstname.Contains(searchQuery) ||
                            b.Lastname.Contains(searchQuery) ||
                            b.BorrowerAddress.Contains(searchQuery))
                .ToList();
            // i used the searchQuery parameter to specify the firstname, lastname or the address to foollow 

            if (borrowers.Count > 0)
            {
                return Ok(borrowers);
            }
            else
            {
                return NotFound();
            }
        }


        //Select all existing borrowers in the database
        [HttpGet("GetBorrowers")]
        public async Task<ActionResult<IEnumerable<Borrowers>>> GetBorrowers()
        {
            return await _context.Borrowerss.ToListAsync();
        }


        //Select all borrowers who contains a given word (can be used on first name and last name)
        [HttpGet("contains/{word}")]
        public IActionResult SelectBorrowersWithWord(string word)
        {
            IEnumerable<Borrowers> borrowers = _context.Borrowerss.Where(l => l.Firstname.Contains(word));
            IEnumerable<Borrowers> borrowers2 = _context.Borrowerss.Where(b => b.Lastname.Contains(word));
            if (borrowers.Any() && borrowers2.Any())
            {
                return Ok(borrowers);
                return Ok(borrowers2);
            }
            else
            {
                return NotFound();
            }
        }

        //Select all borrowers who lives in a given adress (Like “Orléans”)
        [HttpGet("lives in/{address}")]
        public IActionResult GetBorrowersByAddress(string address)
        {
            var borrowers = _context.Borrowerss
                .Where(b => b.BorrowerAddress.Contains(address))
                .ToList();

            if (borrowers.Count > 0)
            {
                return Ok(borrowers);
            }
            else
            {
                return NotFound();
            }
        }

        //Select all the borrowers of a book
        [HttpGet("borrowers of/{bookId}")]
        public IActionResult GetBorrowersByBookId(int bookId)
        {
            var borrowers = _context.Borrowerss
                .Where(b => b.ISBN == bookId)
                .ToList();

            if (borrowers.Count > 0)
            {
                return Ok(borrowers);
            }
            else
            {
                return NotFound();
            }
        }

        //Bonus1 
        //count the books loaned by a borrower
        [HttpGet("bonus1: borrowed-books-count/{id}")]
        public IActionResult CountBorrowedBooks(int id)
        {
            var borrower = _context.Borrowerss.FirstOrDefault(b => b.IdBorrowers == id);

            if (borrower != null)
            {
                var borrowedBooksCount = borrower.BooksBorrowed ?? 0;
                return Ok(borrowedBooksCount);
            }
            else
            {
                return NotFound();
            }
        }


        //Bonus2
        // Sélect boorower by phone type (exemple 32,to follow all belgian borrowers 
        [HttpGet("bonus2: borrowers/by-phone-code/{code}")]
        public IActionResult GetBorrowersByPhonePrefix(string code)
        {
            var borrowers = _context.Borrowerss
                .Where(borrower => borrower.Phone != null && borrower.Phone.ToString().StartsWith(code))
                .ToList();

            return Ok(borrowers);
        }










    }
}
