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
        public long IdBooks { get; set; }
        public string? Title { get; set; }
        public string? NameAuthor { get; set; }

        //public virtual ICollection<Borrowers> Borrowers { get; set; }

    }
}
