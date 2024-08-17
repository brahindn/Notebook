﻿using System.ComponentModel.DataAnnotations;

namespace Notebook.Domain.Requests
{
    public record ContactForCreateDTO
    {
        [Required, StringLength(30, ErrorMessage = "First Name length can't be more than 30.")]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(30, ErrorMessage = "Last Name length can't be more than 30.")]
        public string LastName { get; set; } = string.Empty;

        [Required, StringLength(14, ErrorMessage = "Phone number length can't be more than 14.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(256)]
<<<<<<< HEAD
        [EmailAddress(ErrorMessage = "Invalid email address.")]
=======
        [EmailAddress]
        [EmailValidation]
>>>>>>> faff5ce7e8027fa5f241d45474814d0da9fce27c
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
