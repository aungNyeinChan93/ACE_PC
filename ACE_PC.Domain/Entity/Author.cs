using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Todo>? Todos { get; set; }
    }
}
