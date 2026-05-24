using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class Todo
    {
        [Key]
        public int TodoId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;


        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }


        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public Author? Author { get; set; }


    }
}
