using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.SQLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ApiRestContext _context;

        public LoanController(ApiRestContext context)
        {
            _context = context;
        }


        //Borrow a book
        [HttpPost("BorrowBooks")]
        public IActionResult BorrowBook(int borrowerId, int bookId)
        {
            Loan loan = new Loan
            {
                IdBorrowers = borrowerId,
                ISBN = bookId,
                StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                EndDate = null 
            };

            
            _context.Loans.Add(loan);
            _context.SaveChanges();

            return Ok();
        }

        //Return a book (End of loan)
        [HttpPost("ReturnBooks")]
        public IActionResult ReturnBook(int loanId)
        {
            Loan loan = _context.Loans.FirstOrDefault(l => l.IdLoan == loanId);

            if (loan != null)
            {
                // i update the EndDate of the loan
                loan.EndDate = DateTime.Now.ToString("yyyy-MM-dd");

                // i backup the modifications
                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        //Modify a borrowed book
        [HttpPut("book/{loanId}")]
        public IActionResult ModifyBorrowedBook(int loanId, [FromBody] int newBookId)
        {
            Loan loan = _context.Loans.FirstOrDefault(l => l.IdLoan == loanId);

            if (loan != null)
            {
                Books newBook = _context.Bookss.FirstOrDefault(b => b.ISBN == newBookId);

                if (newBook != null)
                {
                    Books oldBook = _context.Bookss.FirstOrDefault(b => b.ISBN == loan.ISBN);

                    if (oldBook != null)
                    {
                        // i update the boorowed book
                        loan.ISBN = newBookId;

                        // i backup
                        _context.SaveChanges();

                        return Ok();
                    }
                }
            }

            return NotFound();
        }

        //Delete a book loan
        [HttpDelete("{loanId}")]
        public IActionResult DeleteLoan(int loanId)
        {
            Loan loan = _context.Loans.FirstOrDefault(l => l.IdLoan == loanId);

            if (loan != null)
            {
                _context.Loans.Remove(loan);
                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        //Get all books borrowed on a given date
        [HttpGet("borrowed books By Date/{date}")]
        //the format of the date is yyyy-mm-dd
        public IActionResult GetBorrowedBooksByDate(DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");

            var borrowedBooks = _context.Loans
                .Where(loan => loan.StartDate == formattedDate)
                .Join(_context.Bookss, loan => loan.ISBN, book => book.ISBN, (loan, book) => book)
                .ToList();

            return Ok(borrowedBooks);
        }

        //Get all books borrowed between a start date and an end date
        [HttpGet("borrowed books between dates")]
        public IActionResult GetBorrowedBooksBetweenDates([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return BadRequest("startDate and endDate must be provided.");
            }

            DateTime parsedStartDate;
            DateTime parsedEndDate;

            if (!DateTime.TryParse(startDate, out parsedStartDate) || !DateTime.TryParse(endDate, out parsedEndDate))
            {
                return BadRequest("Invalid date format. Please use the format 'yyyy-MM-dd'.");
            }

            var allLoans = _context.Loans.ToList();

            var borrowedBooks = allLoans
                .Where(loan => DateTime.TryParse(loan.StartDate, out var loanStartDate) &&
                               (string.IsNullOrEmpty(loan.EndDate) || (DateTime.TryParse(loan.EndDate, out var loanEndDate) && loanEndDate <= parsedEndDate)) &&
                               loanStartDate >= parsedStartDate) // i  included the books thats was not even end 
                .Join(_context.Bookss, loan => loan.ISBN, book => book.ISBN, (loan, book) => book)
                .ToList();

            return Ok(borrowedBooks);
        }

        //****************something is wrong 🫡***********************
        //Get all books that are past their return date
        [HttpGet("return date pasted books")]
        public IActionResult GetOverdueBooks()
        {
            DateTime currentDate = DateTime.Now;

            var overdueBooks = _context.Loans
                .AsEnumerable()
                .Where(loan => DateTime.TryParseExact(loan.ReturnDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime returnDate) &&
                               returnDate < currentDate &&
                               string.IsNullOrEmpty(loan.EndDate))
                .Join(_context.Bookss, loan => loan.ISBN, book => book.ISBN, (loan, book) => book)
                .ToList();

            return Ok(overdueBooks);
        }


        //Get all books that are loan by a borrower
        [HttpGet("borrowed books/{borrowerId}")]
        public IActionResult GetBorrowedBooks(long borrowerId)
        {
            var borrowedBooks = _context.Loans
                .Where(loan => loan.IdBorrowers == borrowerId)
                .Join(_context.Bookss, loan => loan.ISBN, book => book.ISBN, (loan, book) => book)
                .ToList();

            return Ok(borrowedBooks);
        }

        //Bonus
        //Bonus 1: all the loans
        [HttpGet("all loans")]
        public IActionResult GetAllLoans()
        {
            var allLoans = _context.Loans.ToList();

            return Ok(allLoans);
        }


        //Bonus 2 : all expected returns today
        [HttpGet("expected returns today")]
        public IActionResult GetExpectedReturnsToday()
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd");

            var expectedReturns = _context.Loans
                .Where(loan => loan.EndDate == formattedDate)
                .Join(_context.Bookss, loan => loan.ISBN, book => book.ISBN, (loan, book) => book)
                .ToList();

            return Ok(expectedReturns);
        }

        [HttpGet("returns-today")]
        public IActionResult GetReturnsToday()
        {
            DateTime today = DateTime.Now.Date;

            var returnsToday = _context.Loans
                .Where(loan => loan.EndDate != null)
                .Include(loan => loan.Borrower) 
                .AsEnumerable() 
                .Where(loan => !string.IsNullOrEmpty(loan.EndDate) && DateTime.TryParseExact(loan.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate) && endDate.Date == today)  // i verifie if loan.EndDate is correct and correspond to the right format
                .Select(result => new
                {
                    FirstName = result.Borrower.Firstname,
                    LoanId = result.IdLoan,
                    BookId = result.ISBN,
                    ReturnDate = result.EndDate
                })
                .ToList();

            return Ok(returnsToday);
        }




        // Bonus 1: count the books loaned by a borrower
        [HttpGet("bonus1: borrowers/loaned-books-count/{borrowerId}/")]
        public IActionResult GetLoanedBooksCount(long borrowerId)
        {
            int loanedBooksCount = _context.Loans.Count(loan => loan.IdBorrowers == borrowerId);
            string result = $"the number of borrowed books for {borrowerId} is : {loanedBooksCount}";

            return Ok(result);
        }

       

        // Bonus 2: all borrower of a book
        [HttpGet("bonus2: books/borrowers/{bookId}")]
        public IActionResult GetBookBorrowers(long bookId)
        {
            var bookBorrowers = _context.Loans
                .Where(loan => loan.ISBN == bookId)
                .Include(loan => loan.Borrower)
                .Select(loan => loan.Borrower)
                .ToList();
            string result = $"the num of borrowers of a book {bookId} is : {bookBorrowers}";

            //return Ok(result);
            return Ok(bookBorrowers);
        }


    }








}

