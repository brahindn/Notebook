namespace Notebook.Domain.Requests
{
    public record GetAddressRequest
    {
        public Guid? Id { get; set; }
        public Guid? ContactId { get; set; }
        public AddressType? AddressType { get; set; }
        public string? Country {  get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? BuildingNumber { get; set; }
    }
}
