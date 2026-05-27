using System.ComponentModel.DataAnnotations;

namespace ACE_PC.BlazorServer.Dtos.Quotes
{
   
    public class CreateQuoteDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
