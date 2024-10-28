namespace Notebook.Domain.Requests
{
    public record GetContactRequest
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
