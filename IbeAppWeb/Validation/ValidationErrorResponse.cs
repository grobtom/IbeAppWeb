namespace IbeAppWeb.Validation;

public class ValidationErrorResponse
{
    public List<ValidationErrorItem> Errors { get; set; } = new ();
}
