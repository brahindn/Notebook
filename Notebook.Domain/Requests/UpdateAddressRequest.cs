using Notebook.Domain;
using System.ComponentModel.DataAnnotations;

namespace Notebook.Domain.Requests
{
    public record UpdateAddressRequest
    {
        public Guid Id { get; set; }

        [EnumDataType(typeof(AddressType))]
        public AddressType? AddressType { get; set; }

        [StringLength(30)]
        public string? Country { get; set; }

        [StringLength(30)]
        public string? City { get; set; }

        [StringLength(30)]
        public string? Region { get; set; }

        [StringLength(50)]
        public string? Street { get; set; }
        public int? BuildingNumber { get; set; }
    }
}
