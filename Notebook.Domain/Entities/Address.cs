using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Domain.Entities
{
    public class Address
    {
        [Key]
        public Guid Id {  get; set; }
        public Guid PersonId { get; set; }
        [ForeignKey ("PersonId")]
        public Contact Person { get; set; }
        public AddressType AddressType {  get; set; }
        public string Country {  get; set; }
        public string Region {  get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber {  get; set; }
    }
}
