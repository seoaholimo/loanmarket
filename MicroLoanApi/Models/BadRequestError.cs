namespace BeyondIT.MicroLoan.Api.Models
{
    public class BadRequestError
    {
        public BadRequestError(string error)
        {
            Error = error;
        }

        public string Error { get; }
    }
}
