
using System.ComponentModel.DataAnnotations;

namespace Notebook.Domain.Requests
{
    public record AddressForCreateDTO
    {
        [Required, EnumDataType(typeof(AddressType))]
        public AddressType AddressType { get; set; }


        [Required, StringLength(30)]
        public string Country { get; set; }


        [Required, StringLength(30)]
        public string City { get; set; }


        [StringLength(30)]
        public string? Region { get; set; }


        [Required, StringLength(50)]
        public string Street { get; set; }


        [Required]
        public int BuildingNumber { get; set; }


        [Required]
        public Guid ContactId { get; set; }
    }
}
