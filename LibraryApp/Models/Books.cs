using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Models
{
    public class Books
    {
        [Key]
        public int ISBN { get; set; }
        public string? Title { get; set; }
        public string? NameAuthor { get; set; }
        public int? Price { get; set; } //Bonus add attribute price
        public string? YearPublication { get; set; }
        public int? NumberPages { get; set; }
        public int? Stock { get; set; }
        public string? Type { get; set; } //Bonus add attribute type

        //public virtual ICollection<Borrowers> Borrowers { get; set; }

    }
}
