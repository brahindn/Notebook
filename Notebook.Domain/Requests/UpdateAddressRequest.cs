using Notebook.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Notebook.Domain.Requests
{
    public record UpdateAddressRequest
    {
        [Required, JsonPropertyName("id")]
        public Guid Id { get; set; }

        [EnumDataType(typeof(AddressType)), JsonPropertyName("addressType")]
        public AddressType? AddressType { get; set; }

        [StringLength(30), JsonPropertyName("country")]
        public string? Country { get; set; } = string.Empty;

        [StringLength(30), JsonPropertyName("city")]
        public string? City { get; set; } = string.Empty;

        [StringLength(30), JsonPropertyName("region")]
        public string? Region { get; set; } = string.Empty;
        [StringLength(50), JsonPropertyName("street")]
        public string? Street { get; set; } = string.Empty;
        [JsonPropertyName("buildingNumber")]
        public int? BuildingNumber { get; set; } = 0;
    }
}
