using System.ComponentModel.DataAnnotations;

namespace Notebook.Domain.Requests
{
    public record CreateAddressRequest
    {
        [Required]
        public Guid ContactId { get; set; }

        [Required, EnumDataType(typeof(AddressType))]
        public AddressType AddressType { get; set; }

        [Required, StringLength(30, ErrorMessage = "Country Name length can't be more than 30.")]
        public string Country { get; set; } = string.Empty;

        [Required, StringLength(30, ErrorMessage = "Region Name length can't be more than 30.")]
        public string Region { get; set; } = string.Empty;

        [Required, StringLength(30, ErrorMessage = "City Name length can't be more than 30.")]
        public string City { get; set; } = string.Empty;

        [Required, StringLength(50, ErrorMessage = "Street Name length can't be more than 30.")]
        public string Street { get; set; } = string.Empty;

        [Required]
        public int BuildingNumber { get; set; } = 0;
    }
}
