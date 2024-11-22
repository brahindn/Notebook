using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Domain.Entities
{
    public class Address
    {
        [Key]
        public Guid Id {  get; set; }
        public AddressType AddressType {  get; set; }
        public string Country {  get; set; }
        public string Region {  get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber {  get; set; }

        [ForeignKey("ContactId")]
        public Guid ContactId { get; set; }

        public Contact? Contact { get; set; }
    }
}
