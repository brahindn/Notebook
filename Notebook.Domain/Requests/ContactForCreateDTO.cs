
using System.ComponentModel.DataAnnotations;

namespace Notebook.Domain.Requests
{
    public record ContactForCreateDTO
    {
        [Required, StringLength(30)]
        public string FirstName { get; set; }

        [Required, StringLength(30)]
        public string LastName { get; set; }

        [Required, StringLength(14)]
        public string PhoneNumber { get; set; }

        [StringLength(256)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
