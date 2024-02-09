using System.ComponentModel.DataAnnotations;

namespace Notebook.Domain.Entities
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
