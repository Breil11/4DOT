using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryApp.Models;

namespace WebApi.Models
{
    public class Loan
    {
        [Key]
        public long IdLoan { get; set; }

        [ForeignKey("Borrower")]
        public long IdBorrowers { get; set; }
        public virtual Borrowers Borrower { get; set; }

        [ForeignKey("Book")]
        public long ISBN { get; set; }
        public virtual Books Book { get; set; }
        public String? StartDate { get; set; }
        public String? EndDate { get; set; }  // here i mean is the date when the borrower is suppose to back the book
        public String? ReturnDate { get; set; } // here i mean is the date when the borrower backed the book

        //public virtual Books? Books { get; set; }

        //public virtual Borrowers? Borrowers { get; set; }
    }
}
