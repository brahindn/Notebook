
using System.Text.Json.Serialization;

namespace Notebook.Domain.Responses
{
    public record GetAddressResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("addressType")]
        public AddressType AddressType { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("street")]
        public string Street { get; set; }
        [JsonPropertyName("buildingNumber")]
        public int BuildingNumber { get; set; }
        [JsonPropertyName("contactId")]
        public Guid ContactId { get; set; }
    }
}
