using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Notebook.Domain.Requests
{
    public record UpdateContactRequest
    {
        [Required, JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required, StringLength(30, ErrorMessage = "First Name length can't be more than 30."), JsonPropertyName("firstName")]
        public string? FirstName { get; set; } = string.Empty;

        [Required, StringLength(30, ErrorMessage = "Last Name length can't be more than 30."), JsonPropertyName("lastName")]
        public string? LastName { get; set; } = string.Empty;

        [Required, StringLength(14, ErrorMessage = "Phone number length can't be more than 14."), JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [StringLength(256), JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [JsonPropertyName("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
