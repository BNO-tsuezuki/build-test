namespace evotool.Models
{
    public class ErrorMessageDto
    {
        public string Message { get; set; }

        public ErrorMessageDto(string message)
        {
            Message = message;
        }
    }
}
