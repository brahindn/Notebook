
using System.Text.Json.Serialization;

namespace Notebook.Domain.Responses
{
    public record GetContactResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
