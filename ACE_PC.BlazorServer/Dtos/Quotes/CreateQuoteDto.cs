namespace ACE_PC.BlazorServer.Dtos.Quotes
{
   
    public class CreateQuoteDto
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int CategoryId { get; set; }
    }
}
