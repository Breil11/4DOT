using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Borrowers
    {
        [Key]
        public long IdBorrowers { get; set; }
        public long ISBN { get; set; } //THE isbn IS THE ID OF THE BOOK
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? BorrowerAddress { get; set; }

        public long? Phone { get; set; }
        public string? Email { get; set; }
        public int? BooksBorrowed { get; set; }



        //public virtual Books? Books { get; set; }
    }

    public class Address
    {
        public string? Number { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? Land { get; set; }
    }
}
